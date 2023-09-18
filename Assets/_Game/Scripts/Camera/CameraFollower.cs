using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollower : Singleton<CameraFollower>
{
    [SerializeField] private List<Camera> cameraList;
    [SerializeField] private Camera currentCamera;
    [SerializeField] private CameraState state;
    [SerializeField] private float cameraMoveSpeed;
    public Transform tf;
    public Transform targetTf;
    
    [Title("Current Position and Rotation")]
    [SerializeField] private Vector3 currentPositionOffset; 
    [SerializeField] private Quaternion currentRotationOffset;
   
    [Title("In Game Position and Rotation")]
    [SerializeField] private Vector3 inGamePositionOffset;
    [SerializeField] private Vector3 inGameRotationOffset;
    
    [Title("Main Menu Position and Rotation")]
    [SerializeField] private Vector3 mainMenuPositionOffset;
    [SerializeField] private Vector3 mainMenuRotationOffset;
    
    [Title("Shop Position and Rotation")]
    [SerializeField] private Vector3 shopPositionOffset;
    [SerializeField] private Vector3 shopRotationOffset;

    private void Awake()
    {
        ChangeState(CameraState.MainMenu);
    }

    private void FixedUpdate()
    {
        tf.position = Vector3.Lerp(tf.position, targetTf.position + currentPositionOffset, Time.deltaTime * cameraMoveSpeed);
        tf.rotation = Quaternion.Lerp(tf.rotation, currentRotationOffset, Time.deltaTime * cameraMoveSpeed);
    }

    public void ChangeState(CameraState stateI)
    {
        switch (stateI)
        {
            case CameraState.MainMenu:
                currentPositionOffset = mainMenuPositionOffset;
                currentRotationOffset = Quaternion.Euler(mainMenuRotationOffset);
                EnableCamera(CameraState.MainMenu);
                break;
            case CameraState.InGame:
                currentPositionOffset = inGamePositionOffset;
                currentRotationOffset = Quaternion.Euler(inGameRotationOffset);
                EnableCamera(CameraState.InGame);
                break;
            case CameraState.Shop:
                currentPositionOffset = shopPositionOffset;
                currentRotationOffset = Quaternion.Euler(shopRotationOffset);
                // Temp
                EnableCamera(CameraState.Shop);
                break;
            default:
                Debug.Log("No State");
                break;
        }
    }
    
    public bool IsObjectInCameraView(Transform target)
    {
        Vector3 screenPoint = currentCamera.WorldToViewportPoint(target.position);
        return screenPoint.x is > 0 and < 1 && screenPoint.y is > 0 and < 1;
    }

    private void EnableCamera(CameraState cameraState)
    {
        cameraList.ForEach(c => c.enabled = false);
        cameraList[(int)cameraState].transform.position = currentCamera.transform.position;
        cameraList[(int)cameraState].transform.rotation = currentCamera.transform.rotation;
        currentCamera = cameraList[(int) cameraState];
        currentCamera.enabled = true;
        tf = currentCamera.transform;
    }

    public Camera GetCamera(CameraState cameraState)
    {
        return cameraList[(int)cameraState];
    }
}

// VER2: Lock X position

// public class CameraFollower : Singleton<CameraFollower>
// {
//     public Transform TF;
//     public Transform playerTF;
//     [SerializeField] private Camera camera;
//     [SerializeField] Vector3 offset;
//
//     private Vector3 _position;
//     private void Awake()
//     {
//         _position = TF.position;
//     }
//     
//     private Vector3 _camToPlayerPos;
//     private void LateUpdate()
//     {
//         _camToPlayerPos = new Vector3(TF.position.x, playerTF.position.y, playerTF.position.z);
//         TF.position = Vector3.Lerp(TF.position, _camToPlayerPos + offset, Time.deltaTime * 5f);
//     }
//     
//     public bool IsObjectInCameraView(Transform target)
//     {
//         Vector3 screenPoint = camera.WorldToViewportPoint(target.position);
//         return screenPoint.x is > 0 and < 1 && screenPoint.y is > 0 and < 1;
//     }
// }

