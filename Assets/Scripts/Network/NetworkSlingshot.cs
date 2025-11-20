using System;
using System.Collections;
using System.Collections.Generic;
using AngryBirds.Birds;
using AngryBirds.Managers;
using AngryBirds.SO.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AngryBirds.Network
{
    public class NetworkSlingshot : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(OnNetworkPositionChanged))] private Vector3 NetworkPosition {get; set;}

        [Networked, OnChangedRender(nameof(OnNetworkBirdChanged))] private NetworkObject NetworkBird {get; set;}
        
        public static event Action<NetworkSlingshot> OnShotFired;
        
        [Header("Slingshot configuration")] 
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _launchPoint;
        [SerializeField] private float _maxStretch = 3f;
        [SerializeField] private float _powerMultiplier = 30f;
        private Rigidbody _currentProjectile;
        private SpringJoint _joint;
    
        [Header("Flight Camera")] 
        [SerializeField] private CinemachineCamera _flightCamera;
        [SerializeField] private float _waitSecondsAfterShot = 2f;

        [Header("Bands Visuals")] 
        [SerializeField] private LineRenderer _leftBand;
        [SerializeField] private LineRenderer _rightBand;
        [SerializeField] private Transform _leftAnchor;
        [SerializeField] private Transform _rightAnchor;

        [Header("Flight Path")] 
        [SerializeField] private LineRenderer _flightPath;
        [SerializeField] private int _pathResolution = 30;
        [SerializeField] private float _pathTimeStep = 0.1f;

        [Header("Network")]
        private NetworkObject _currentProjectilePrefab;
        private Vector3 _syncedPosition;
        
        [Header("Ammo")]
        private BirdsAmmoSO _birdsList;
        
        [Header("Dragging")]
        private bool _isDragging = false;
        private bool _canDrag = false;
        private DraggingInputActions _inputActions;    
        
        private void Awake()
        {
            _inputActions = new DraggingInputActions();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CameraManager.EnableSlingshot += EnableInput;
            _inputActions.Drag.DragAndMove.started += OnDragStarted;
            _inputActions.Drag.PointerPosition.performed += OnDragPerformed;
            _inputActions.Drag.DragAndMove.canceled += OnDragCanceled;
            _inputActions.Enable();
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            CameraManager.EnableSlingshot -= EnableInput;
            _inputActions.Drag.DragAndMove.started -= OnDragStarted;
            _inputActions.Drag.PointerPosition.performed -= OnDragPerformed;
            _inputActions.Drag.DragAndMove.canceled -= OnDragCanceled;
            _inputActions.Disable();
        }

        private void Start()
        {
            _leftBand.enabled = true;
            _rightBand.enabled = true;

            _syncedPosition = _launchPoint.position;
            _inputActions.Disable();
        }
   
        private void LateUpdate()
        {
            UpdateBands();
        }
    
        private void OnNetworkPositionChanged()
        {
            if (HasStateAuthority)
            {
                return;
            }
            
            if(_currentProjectile == null) 
            {
                return;
            }
            _currentProjectile.position = NetworkPosition;
        }

        private void OnNetworkBirdChanged()
        {
            _currentProjectilePrefab = NetworkBird;
            CreateProjectile();
        }
        
        public void SetCamera(CinemachineCamera cam, PlayerRef targetPlayer)
        {
            if (HasStateAuthority)
            {
                _flightCamera = cam;
                cam.Follow = _launchPoint;    
            }
            
        } 
        
        public void SetAmmo(BirdsAmmoSO ammo, NetworkId playerId)
        {
            _birdsList = ammo;
            _currentProjectilePrefab = Runner.Spawn(_birdsList.Birds[0].gameObject, _launchPoint.position, _launchPoint.rotation);
           
            CreateProjectile();
            CreateProjectileRpc(_currentProjectilePrefab.GetComponent<NetworkObject>().Id, playerId);
           
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void GetProjectileRpc(NetworkId slingshotId, NetworkId birdId)
        {
            NetworkObject slingshotObj = null;
            NetworkObject birdObj = null;
            if (Runner.TryFindObject(slingshotId, out slingshotObj) &&  Runner.TryFindObject(birdId, out birdObj) )
            {
                _currentProjectilePrefab = birdObj;
                
                _syncedPosition = _launchPoint.position;
                CreateProjectile();
        
                if (HasStateAuthority)
                {
                    _currentProjectilePrefab.transform.position = _launchPoint.position;
                    NetworkBird = birdObj;
                    NetworkPosition = _syncedPosition;
                    CreateProjectileRpc(birdId, slingshotId);
                }
            }
        }
       
        private void CreateProjectile()
        {
            if (HasStateAuthority)
            {
                NetworkPosition = _launchPoint.position;
            }
            if (_currentProjectilePrefab != null)
            {
                if (_flightCamera != null)
                {
                    _flightCamera.Follow = _launchPoint;
                }
                _currentProjectile = _currentProjectilePrefab.gameObject.GetComponent<Rigidbody>();

                _currentProjectile.transform.SetPositionAndRotation(_launchPoint.position, _currentProjectilePrefab.transform.rotation);

                if (HasStateAuthority)
                {
                    NetworkPosition = _currentProjectile.position;
                }
                
                _joint = _currentProjectile.gameObject.GetComponent<SpringJoint>();
                _joint.connectedAnchor = _pivot.position;
                _joint.autoConfigureConnectedAnchor = false;
                _currentProjectile.isKinematic = true; 
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void CreateProjectileRpc(NetworkId birdId, NetworkId slingshotId)
        {
            NetworkObject slingshotObj = null;
            NetworkObject birdObj = null;
            if (Runner.TryFindObject(slingshotId, out slingshotObj) &&  Runner.TryFindObject(birdId, out birdObj) )
            {
                slingshotObj.GetComponent<NetworkSlingshot>()._currentProjectilePrefab = birdObj;
                CreateProjectile();
            }
        }
        
        private void EnableInput(bool obj)
        {
            if (obj)
            {
                _inputActions.Enable();
            }
            else
            {
                _inputActions.Disable();
            }
        }
        
        private void OnDragCanceled(InputAction.CallbackContext obj)
        {
            if (!HasStateAuthority)
            {
                return;
            }
            if (!_canDrag)
            {
                return;
            }
   
            AudioManager.Instance.PlayLaunchSlingshot();
            _isDragging = false;
            _flightPath.enabled = false;
            StartCoroutine(Release());    
        }

        private void OnDragPerformed(InputAction.CallbackContext obj)
        {
            if (!_canDrag)
            {
                return;
            }
        
            Vector2 screenPos = obj.ReadValue<Vector2>();
            DragProjectile(screenPos);
            UpdateFlightPath();
        }

        private void OnDragStarted(InputAction.CallbackContext obj)
        {
            if (!HasStateAuthority)
            {
                return;
            }
           
            if (IsPointerOverUIObject())
            {
                _canDrag = false;
                return;
            }
         
            if (_currentProjectile == null || _currentProjectilePrefab == null)
            {
                return;
            }

            _canDrag = true; 
            AudioManager.Instance.PlaySlingshotStretch();
            AudioManager.Instance.PlaySelectedBirdsSoundEffects(_currentProjectilePrefab.GetComponent<BirdBase>().BirdType);
            _isDragging = true;
        }
    
        private bool IsPointerOverUIObject() 
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        
        private void DragProjectile(Vector2 screenPos)
        {
            if (_currentProjectile == null)
            {
                return;
            }

            if (!_currentProjectile.isKinematic)
            {
                _currentProjectile.isKinematic = true;
            }

            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Plane plane = new Plane(Vector3.up, _pivot.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 point = ray.GetPoint(distance);
                Vector3 dir = point - _pivot.position;

                if (dir.magnitude > _maxStretch)
                    dir = dir.normalized * _maxStretch;

                _currentProjectile.position = _pivot.position + dir;

                if (HasStateAuthority)
                {
                    NetworkPosition = _currentProjectile.position;
                }

            }
        }

        private IEnumerator Release()
        {
            _canDrag = false;
            _currentProjectilePrefab = null;
            if (HasStateAuthority)
            {
                NetworkPosition = _currentProjectile.position;
            }            
            
            if (_currentProjectile == null)
            {
                yield break;
            }
            
            _syncedPosition = _currentProjectile.position;
          
            if (Runner.IsSharedModeMasterClient)
            {
                ApplyForce(this.GetComponent<NetworkObject>());
            }
            else
            {
                ApplyForceRpc(this.GetComponent<NetworkObject>());
            }
            CreateProjectile();
        }
        
        private void ReleaseBird()
        {
            if(_currentProjectile != null) 
            {
                _canDrag = false;
                _currentProjectilePrefab = null;

                Destroy(_joint);
                _currentProjectile.isKinematic = false;
                _currentProjectile.gameObject.GetComponent<BirdBase>().OnShoot?.Invoke();

                Vector3 forceDir = (_pivot.position - _currentProjectile.position);
                float stretch = forceDir.magnitude / _maxStretch;
                float heightBoost = 0.5f + stretch * 2f;
                forceDir.y += heightBoost;

                float forceMag = forceDir.magnitude * _powerMultiplier;
                _currentProjectile.AddForce(forceDir.normalized * forceMag, ForceMode.Impulse);
                _currentProjectile = null;
                RequestNewBird();
            }
        }

        private void RequestNewBird()
        {
            SharedModeMasterClientTracker.RequestBird(this, Runner.LocalPlayer);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void ApplyForceRpc(NetworkObject slingshot)
        {
            ApplyForce(slingshot);
        }
        
        private void ApplyForce(NetworkObject slingshot)
        {
            if (Runner.IsSharedModeMasterClient)
            {
                slingshot.GetComponent<NetworkSlingshot>().ReleaseBird();
            }
        }

        private void UpdateBands()
      {
            if (_leftBand && _rightBand)
            {
                _leftBand.enabled = true;
                _rightBand.enabled = true;
                
                Vector3 pos = _launchPoint.position;
                
                if(!HasStateAuthority)
                {
                    pos = NetworkPosition;
                }
                else if (HasStateAuthority && _currentProjectile != null)
                {
                    pos = _currentProjectile.position;
                }
                
                _leftBand.SetPosition(0, _leftAnchor.position);
                _leftBand.SetPosition(1, pos);

                _rightBand.SetPosition(0, _rightAnchor.position);
                _rightBand.SetPosition(1, pos);

                float stretch = Vector3.Distance(pos, _pivot.position) / _maxStretch;
                float width = Mathf.Lerp(0.08f, 0.02f, stretch);
                _leftBand.startWidth = _leftBand.endWidth = width;
                _rightBand.startWidth = _rightBand.endWidth = width;
            }
        }

        private void UpdateFlightPath()
        {
            if (_flightPath == null || _currentProjectile == null || !_isDragging)
            {
                return;
            }

            _flightPath.enabled = true;
            _flightPath.positionCount = _pathResolution;

            Vector3 startPos = _currentProjectile.position;
            Vector3 forceDir = (_pivot.position - _currentProjectile.position);
            float stretch = forceDir.magnitude / _maxStretch;
            float heightBoost = 0.5f + stretch * 2f;
            forceDir.y += heightBoost;
            float forceMag = forceDir.magnitude * _powerMultiplier;
            Vector3 initialVelocity = forceDir.normalized * forceMag / _currentProjectile.mass;

            for (int i = 0; i < _pathResolution; i++)
            {
                float t = i * _pathTimeStep;
                Vector3 pos = startPos + initialVelocity * t + 0.5f * Physics.gravity * t * t;
                _flightPath.SetPosition(i, pos);
            }
        }
    }
}