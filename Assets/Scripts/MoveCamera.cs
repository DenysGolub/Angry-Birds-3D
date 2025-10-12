using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCamera : MonoBehaviour
{
    public float MoveSpeed = 0.5f;
    private bool _isDragging = false;   
    public Vector2Int LevelBorders = new Vector2Int(0, 30);
    
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            _isDragging = true;
        }

        if (_isDragging)
        {
            float mouseX = 0f;

#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
            mouseX = Input.GetAxis("Mouse X");
            #elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                    mouseX = touch.deltaPosition.x / Screen.width; 
            }
#endif

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

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }
}
