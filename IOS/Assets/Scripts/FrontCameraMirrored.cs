using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
 
[RequireComponent(typeof(Camera))]
public class FrontCameraMirrored : MonoBehaviour
{

    private bool mFlipped = false;
    private bool mFlipVertical = true;
    private bool mFlipHorizontal = true;
    private bool mFirstCall = true;

    private CameraDevice.CameraDirection direction;
    private Transform backgroundPlane;
    private Vector3 planeRearCam;
    private Vector3 planeFrontCam;

    private bool firstRun = true;

    void Start()
    {
        backgroundPlane = transform.GetComponentInChildren<Transform>();
        float x = backgroundPlane.transform.localScale.x;
        float y = backgroundPlane.transform.localScale.y;
        float z = backgroundPlane.transform.localScale.z;

        planeRearCam = new Vector3(x, y, z);
        //planeFrontCam = new Vector3(x * -1, y * -1, z);
        planeFrontCam = new Vector3(x, y * -1, z);
    }

    void OnPreCull()
    {
        if (CameraDevice.Instance != null)
        {
            direction = CameraDevice.Instance.GetCameraDirection();
            switch (direction)
            {
                case CameraDevice.CameraDirection.CAMERA_FRONT:
                    if (firstRun)
                    {
                        backgroundPlane.localScale = planeFrontCam;
                        firstRun = false;
                    }
                    break;

                case CameraDevice.CameraDirection.CAMERA_BACK:
                    backgroundPlane.localScale = planeRearCam;
                    mFlipped = false;
                    mFirstCall = true;
                    break;
            }
        }
        else
        {
            Debug.Log("<color=cyan>CameraDevice.Instance = null</color>");
            return;
        }

        if (!mFlipped && direction == CameraDevice.CameraDirection.CAMERA_FRONT)
        {
            if (!mFirstCall)
            {  // don't flip on first call to OnPreCull()

                Camera cam = this.GetComponent<Camera>();
                Vector3 flipScale = new Vector3(mFlipHorizontal ? -1 : 1, mFlipVertical ? -1 : 1, 1);
                Matrix4x4 projMat = cam.projectionMatrix * Matrix4x4.Scale(flipScale);
                cam.projectionMatrix = projMat;
                mFlipped = true;
            }
            mFirstCall = false;
        }
    }

    void OnPreRender()
    {
        if (direction == CameraDevice.CameraDirection.CAMERA_FRONT)
        {
            if ((mFlipVertical && !mFlipHorizontal) ||
                (mFlipHorizontal && !mFlipVertical))
            {
                GL.SetRevertBackfacing(true);
            }
        }
    }

    // Set it to false again because we don't want to affect all other cameras.
    void OnPostRender()
    {
        if (direction == CameraDevice.CameraDirection.CAMERA_FRONT)
        {
            if ((mFlipVertical && !mFlipHorizontal) ||
                (mFlipHorizontal && !mFlipVertical))
            {
                GL.SetRevertBackfacing(false);
            }
        }
    }
}