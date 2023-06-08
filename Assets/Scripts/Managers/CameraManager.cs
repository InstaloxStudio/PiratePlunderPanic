using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;
    public Camera currentCamera;

    public CinemachineVirtualCamera vCam;

    //virtual camera body transposer
    public CinemachineTransposer cinemachineTransposer;


    private void Awake()
    {
        instance=this;
    }


    public void SetCamera(Camera camera)
    {
        currentCamera = camera;
    }

    public Camera GetCamera()
    {
          return currentCamera;
    }

    public CinemachineTransposer GetCinemachineTransposer()
    {
        var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        return transposer;

    }

    public void SetVCamFollowTarget(Transform target)
    {
        vCam.Follow = target;
    }

    public void SetVCamLookAtTarget(Transform target)
    {
        vCam.LookAt = target;
    }


}
