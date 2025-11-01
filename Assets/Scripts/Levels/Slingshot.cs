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

namespace AngryBirds.Levels
{
    public class Slingshot : MonoBehaviour
    {
        [Header("Slingshot setup")] 
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _launchPoint;
        [SerializeField] private float _maxStretch = 3f;
        [SerializeField] private float _powerMultiplier = 30f;
    
        [Header("Flight Camera Setup")] 
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

        [Header("First ammo setup")]
        [SerializeField]
        private BirdsAmmoSO _birdsList;
        
        private GameObject _currentProjectilePrefab;
 
        private Rigidbody _currentProjectile;
        private SpringJoint _joint;
        private bool _isDragging = false;
        private bool _canDrag = false;
        private DraggingInputActions _inputActions;    
    
        public static event Action OnShotFired;
        
        private void Awake()
        {
            _currentProjectilePrefab = Instantiate(_birdsList.Birds[0].gameObject);
           
            CreateProjectile();
            _inputActions = new DraggingInputActions();
        }

        
        private void OnEnable()
        {
            GameManager.SetNextBirdToSlingshotAction += GetProjectile;
            GameManager.OnGameOver += DisableSlingshotOnGameOver;
            CameraManager.EnableSlingshot += EnableInput;
            _inputActions.Drag.DragAndMove.started += OnDragStarted;
            _inputActions.Drag.PointerPosition.performed += OnDragPerformed;
            _inputActions.Drag.DragAndMove.canceled += OnDragCanceled;
            _inputActions.Enable();

        }
    
        private void OnDisable()
        {
            GameManager.SetNextBirdToSlingshotAction -= GetProjectile;
            GameManager.OnGameOver -= DisableSlingshotOnGameOver;
            CameraManager.EnableSlingshot -= EnableInput;

            _inputActions.Drag.DragAndMove.started -= OnDragStarted;
            _inputActions.Drag.PointerPosition.performed -= OnDragPerformed;
            _inputActions.Drag.DragAndMove.canceled -= OnDragCanceled;
            _inputActions.Disable();

        }

        private void Start()
        {
            _inputActions.Drag.Disable();
        }

   
        private void EnableInput(bool obj)
        {
            Debug.Log($"Enabled input! {obj}");
            if (obj)
            {
                _inputActions.Drag.Enable();
            }
            else
            {
                _inputActions.Drag.Disable();
            }
        }
        private void LateUpdate()
        {
            UpdateBands();
        }

        private void OnDragCanceled(InputAction.CallbackContext obj)
        {
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
            // if (!Object.HasStateAuthority)
            //{
              //  return;
           // }

            if (IsPointerOverUIObject())
            {
                _canDrag = false;
                return;
            }
         
            if (_currentProjectile == null)
            {
                return;
            }

            _canDrag = true; 
            AudioManager.Instance.PlaySlingshotStretch();
            AudioManager.Instance.PlaySelectedBirdsSoundEffects(_currentProjectilePrefab.GetComponent<BirdBase>().BirdType);
            _isDragging = true;
        }
    
        private bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        private void DisableSlingshotOnGameOver(bool value)
        {
            this.enabled = false;
        }

        private void GetProjectile(GameObject bird)
        {
            _currentProjectilePrefab = bird;
        }

        private void CreateProjectile()
        {
            _flightCamera.Follow = _pivot;

            if (_currentProjectilePrefab != null)
            {
                _currentProjectile = _currentProjectilePrefab.gameObject.GetComponent<Rigidbody>();
                _currentProjectile.transform.SetPositionAndRotation(_launchPoint.position, _currentProjectilePrefab.transform.rotation);
                _joint = _currentProjectile.gameObject.GetComponent<SpringJoint>();
                _joint.connectedAnchor = _pivot.position;
                _joint.autoConfigureConnectedAnchor = false;
                _currentProjectile.isKinematic = true; 
            }
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
                
            }
        }

        private IEnumerator Release()
        {
            if (_currentProjectile == null)
            {
                yield break;
            }

            _canDrag = false;
            _currentProjectilePrefab = null;
            OnShotFired?.Invoke();
        
            _currentProjectile.gameObject.GetComponent<BirdBase>().OnShoot?.Invoke();
            Destroy(_joint);
            _currentProjectile.isKinematic = false;

            _flightCamera.Follow = _currentProjectile.transform;

            Vector3 forceDir = (_pivot.position - _currentProjectile.position);
            float stretch = forceDir.magnitude / _maxStretch;
            float heightBoost = 0.5f + stretch * 2f;
            forceDir.y += heightBoost;
        
            float forceMag = forceDir.magnitude * _powerMultiplier;
            _currentProjectile.AddForce(forceDir.normalized * forceMag, ForceMode.Impulse);

            _currentProjectile = null;

            yield return new WaitForSeconds(_waitSecondsAfterShot);
            CreateProjectile();
        }

        private void UpdateBands()
        {
            if (_leftBand && _rightBand && _currentProjectile)
            {
                _leftBand.enabled = true;
                _rightBand.enabled = true;

                _leftBand.SetPosition(0, _leftAnchor.position);
                _leftBand.SetPosition(1, _currentProjectile.position);

                _rightBand.SetPosition(0, _rightAnchor.position);
                _rightBand.SetPosition(1, _currentProjectile.position);

                float stretch = Vector3.Distance(_currentProjectile.position, _pivot.position) / _maxStretch;
                float width = Mathf.Lerp(0.08f, 0.02f, stretch);
                _leftBand.startWidth = _leftBand.endWidth = width;
                _rightBand.startWidth = _rightBand.endWidth = width;
            }
            else
            {
                if (_leftBand) _leftBand.enabled = false;
                if (_rightBand) _rightBand.enabled = false;
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