using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    public float MoveSpeed = 0.5f;
    private bool _isDragging = false;   
    public Vector2Int LevelBorders = new Vector2Int(0, 30);
    private DraggingInputActions inputActions;

    void Awake()
    {
        inputActions = new DraggingInputActions();
    }
    

    private void OnEnable()
    {
        inputActions.Drag.DragAndMove.started += OnDragStarted;
        inputActions.Drag.PointerPosition.performed += OnDragPerformed;
        inputActions.Drag.DragAndMove.canceled += OnDragCanceled;
        //inputActions.Gameplay.Shoot.performed += OnShoot;        
        
        inputActions.Enable();
    }

    private void OnDragStarted(InputAction.CallbackContext obj)
    {
        _isDragging = true;
    }

    private void OnDisable()
    {
        inputActions.Drag.DragAndMove.started -= OnDragStarted;
        inputActions.Drag.PointerPosition.performed -= OnDragPerformed;
        inputActions.Drag.DragAndMove.canceled -= OnDragCanceled;
        //inputActions.Gameplay.Shoot.performed += OnShoot;        
        
        inputActions.Disable();
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
                currentPosition.z += MoveSpeed*Time.deltaTime;
                currentPosition.z = Mathf.Clamp(currentPosition.z, LevelBorders.x, LevelBorders.y);
                //Debug.Log(currentPosition);
                transform.localPosition = currentPosition;
            }
            else if (mouseX > 0)
            {
                Debug.Log("Mouse is moving right");
                var currentPosition = transform.localPosition;
                currentPosition.z -= MoveSpeed*Time.deltaTime;
                currentPosition.z = Mathf.Clamp(currentPosition.z, LevelBorders.x, LevelBorders.y);
                transform.localPosition = currentPosition;        
            }
        }
    }
    private Vector2 _lastPointerPos;

    private void Update()
    {
        if (!_isDragging)
            return;

        Vector2 currentPointerPos = ReadPointerPosition();
        float deltaX = currentPointerPos.x - _lastPointerPos.x;
        _lastPointerPos = currentPointerPos;

        var currentPosition = transform.localPosition;
        currentPosition.z -= deltaX * MoveSpeed * Time.deltaTime;
        currentPosition.z = Mathf.Clamp(currentPosition.z, LevelBorders.x, LevelBorders.y);
        transform.localPosition = currentPosition;
    }

    private Vector2 ReadPointerPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }

    private void OnDragCanceled(InputAction.CallbackContext obj)
    {
        _isDragging = false;
    }
}
