using UnityEngine;
using UnityEngine.InputSystem;

namespace AngryBirds.Levels
{
    public class MoveCamera : MonoBehaviour
    {
        private DraggingInputActions _inputActions;
        private bool _isDragging = false;   
        private Vector2 _lastPointerPos;
    
        [SerializeField] private float _moveSpeed = 0.8f; 
        [SerializeField] private Vector2Int LevelBorders = new Vector2Int(0, 19);

        private void Awake()
        {
            _inputActions = new DraggingInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Drag.DragAndMove.started += OnDragStarted;
            _inputActions.Drag.PointerPosition.performed += OnDragPerformed;
            _inputActions.Drag.DragAndMove.canceled += OnDragCanceled;
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Drag.DragAndMove.started -= OnDragStarted;
            _inputActions.Drag.PointerPosition.performed -= OnDragPerformed;
            _inputActions.Drag.DragAndMove.canceled -= OnDragCanceled;
            _inputActions.Disable();
        }
    
        private void Update()
        {
            if (!_isDragging)
            {
                return;
            }

            Vector2 currentPointerPos = ReadPointerPosition();
            float deltaX = currentPointerPos.x - _lastPointerPos.x;
            _lastPointerPos = currentPointerPos;

            var currentPosition = transform.localPosition;
            currentPosition.z -= deltaX * _moveSpeed * Time.deltaTime;
            currentPosition.z = Mathf.Clamp(currentPosition.z, LevelBorders.x, LevelBorders.y);
            transform.localPosition = currentPosition;
        }
    
        private void OnDragStarted(InputAction.CallbackContext obj)
        {
            _isDragging = true;
        }
    
        private void OnDragPerformed(InputAction.CallbackContext obj)
        {
            if (_isDragging)
            {
                float mouseX = obj.ReadValue<Vector2>().x;
                if (mouseX < 0)
                {
                    Debug.Log("Mouse is moving left");
                    var currentPosition = transform.localPosition;
                    currentPosition.z += _moveSpeed*Time.deltaTime;
                    currentPosition.z = Mathf.Clamp(currentPosition.z, LevelBorders.x, LevelBorders.y);
                    transform.localPosition = currentPosition;
                }
                else if (mouseX > 0)
                {
                    Debug.Log("Mouse is moving right");
                    var currentPosition = transform.localPosition;
                    currentPosition.z -= _moveSpeed*Time.deltaTime;
                    currentPosition.z = Mathf.Clamp(currentPosition.z, LevelBorders.x, LevelBorders.y);
                    transform.localPosition = currentPosition;        
                }
            }
        }

        private Vector2 ReadPointerPosition()
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                return Touchscreen.current.primaryTouch.position.ReadValue();
            }
            if (Mouse.current != null)
            {
                return Mouse.current.position.ReadValue();
            }
            return Vector2.zero;
        }

        private void OnDragCanceled(InputAction.CallbackContext obj)
        {
            _isDragging = false;
        }
    }
}
