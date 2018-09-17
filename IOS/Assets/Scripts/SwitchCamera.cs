using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SwitchCamera : MonoBehaviour
{
    public Button switchIcon;
    public Button secondPanelSwitchIcon;
    public GameObject watermark;
    public GameObject tutorText;
    public GameObject ctdnText;
    public static bool isFront;
    //Transform watermarkTrans;
    public static bool needMirrored = false;
    public static bool firstTriggered = true;
    bool firstOpen = true;

    void Start()
    {
        switchIcon.onClick.AddListener(SwitchTheCamera);
        secondPanelSwitchIcon.onClick.AddListener(SwitchTheCamera);

        //CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
        //CameraDevice.Instance.Start();
        //TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
        isFront = false;
        needMirrored = false;
        firstTriggered = true;
        firstOpen = true;
    }
    private void Update()
    {
        if(UIManager.secondToThird == true && firstTriggered == true){
            SwitchTheCamera();
            firstTriggered = false;
        }
    }

    void SwitchTheCamera()
    {
        if (isFront)
        {
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
            needMirrored = false;
            watermark.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //if(firstOpen == true){
            //    watermark.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //    SecondPanel.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //    firstOpen = false;
            //}else{
            //    watermark.transform.localRotation = Quaternion.Euler(0, 180, 0);
            //    SecondPanel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            //}
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

            needMirrored = true;
            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
            CameraDevice.Instance.Start();
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
            watermark.transform.localRotation = Quaternion.Euler(0, 180, 0);
            ctdnText.transform.localRotation = Quaternion.Euler(0, 180, 0);
            tutorText.SetActive(false);
            //SecondPanel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFront = true;
        }
    }


}