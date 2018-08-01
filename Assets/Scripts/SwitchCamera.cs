using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SwitchCamera : MonoBehaviour
{

    //WebCamTexture webCamTexture;
    //WebCamDevice[] devices;
    public Button switchIcon;
    bool isFront;

    void Start()
    {
        Button btnSwitch = switchIcon.GetComponent<Button>();
        btnSwitch.onClick.AddListener(SwitchTheCamera);

        isFront = false;
    }

    void SwitchTheCamera()
    {
        if (isFront)
        {
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_BACK);
            CameraDevice.Instance.Start();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
            isFront = false;

        }
        else if (!isFront)
        {
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
            CameraDevice.Instance.Start();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
            isFront = true;
        }
    }


}