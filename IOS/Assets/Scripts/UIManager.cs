using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //First
    public Image black;
    public Image appIcon;
    public Text authorText;
    public Image watermark;
    Color imageColor;
    Color textColor;
    Color markColor;
    float startTime = -1;

    public GameObject ARModeIcons;
    public GameObject FirstPanel;
    public GameObject SecondPanel;
    public GameObject ThirdPanel;

    //Second
    public Image user;
    Color userColor;
    public GameObject cancelIcon;
    public GameObject secondCorrection;
    public GameObject secondSwitch;
    public GameObject countdownObj;
    Text countdownText;
    public GameObject noticeText;
    float clickStartTime = -1;
    bool countdownThread = false;
    bool shotted = false;

    //Third
    public Button thirdGo;
    //Main
    public Button backToCorrection;
    public GameObject photo;
    //public Text cameraText;

    bool firstDone = false;
    bool firstToSecond = false;
    bool firstTimeToSecond = true;
    //bool firstTimeToSecond = false;
    bool secondDone = false;
    public static bool secondToThird = false;

    void Start()
    {
        FirstPanel.SetActive(true);
        SecondPanel.SetActive(false);
        ThirdPanel.SetActive(false);
        ARModeIcons.SetActive(false);

        //First
        imageColor = black.color;
        imageColor.a = 1;
        textColor = authorText.color;
        textColor.a = 1;
        markColor = watermark.color;
        markColor.a = 0;
        watermark.color = markColor;

        startTime = Time.time;

        //Second
        userColor = user.color;
        userColor.a = 0.8f;
        Button btnCancel = cancelIcon.GetComponent<Button>();
        btnCancel.onClick.AddListener(OnClickCancellation);
        Button btnSecCorrection = secondCorrection.GetComponent<Button>();
        btnSecCorrection.onClick.AddListener(OnClickCorrection);
        countdownText = countdownObj.GetComponent<Text>();
        clickStartTime = -1;
        countdownThread = false;
        shotted = false;

        //Third
        Button btnGo = thirdGo.GetComponent<Button>();
        btnGo.onClick.AddListener(OnClickGo);

        //Main
        Button btnBackCorrect = backToCorrection.GetComponent<Button>();
        btnBackCorrect.onClick.AddListener(OnClickBackToCorrect);

        firstDone = false;
        firstToSecond = false;
        firstTimeToSecond = true;
        //firstTimeToSecond = false;
        secondDone = false;
        secondToThird = false;
    }

    // Update is called once per frame
    void Update()
    {
        //First Panel
        if (firstDone == false)
        {
            if (Time.time - startTime >= 3.0f)
            {
                if (imageColor.a > 0.0f)
                {
                    imageColor.a -= 0.008f;
                    black.color = imageColor;
                }
                if(markColor.a < 1.0f){
                    markColor.a += 0.01f;
                    watermark.color = markColor;
                }
                if (textColor.a > 0.0f)
                {
                    textColor.a -= 0.01f;
                    authorText.color = textColor;
                    appIcon.color = textColor;
                }
                else if (imageColor.a < 0.02f)
                {
                    authorText.enabled = false;
                    black.enabled = false;
                    firstDone = true;
                    firstToSecond = true;
                }
            }
        }
        else if(firstDone == true && firstToSecond == true){
            //Second Panel
            SecondPanel.SetActive(true);
            if (firstTimeToSecond){
                cancelIcon.SetActive(false);
                firstTimeToSecond = false;
            }
            else{cancelIcon.SetActive(true);}
            FirstPanel.SetActive(false);
            countdownObj.SetActive(false);
            firstToSecond = false;
        }
        else if(secondDone == true && secondToThird == true){
            //Load photo as texture
            if(ScreenRecorder.reloadTexture == true){
                Debug.Log("PATH:"+ScreenRecorder.nowUserPhotoFilePath);
                Texture2D newTexture = IMG2Sprite.LoadTexture(ScreenRecorder.nowUserPhotoFilePath);
                photo.GetComponent<Renderer>().sharedMaterial.mainTexture = newTexture;
                ScreenRecorder.reloadTexture = false;
            }

            noticeText.SetActive(true);
            secondCorrection.SetActive(true);
            secondSwitch.SetActive(true);
            cancelIcon.SetActive(true);
            userColor.a = 0.8f;
            user.color = userColor;
            SecondPanel.SetActive(false);
            ThirdPanel.SetActive(true);
            ARModeIcons.SetActive(true);
            secondToThird = false;
        }

        if (countdownThread == true)
        {
            if (clickStartTime == -1)
            {
                clickStartTime = Time.time;
            }
            if(userColor.a > 0.0f){
                userColor.a -= 0.0051f;
                user.color = userColor;
            }
            if (Time.time - clickStartTime < 3.0f)
            {
                noticeText.SetActive(false);
                secondCorrection.SetActive(false);
                secondSwitch.SetActive(false);
                cancelIcon.SetActive(false);
                int countdown = 3 - (int)(Time.time - clickStartTime);
                countdownText.text = countdown.ToString();
            }
            else
            {
                if(shotted == false){
                    SecondPanel.SetActive(false);
                    ScreenRecorder.captureScreenshot = true;
                    shotted = true;
                }
                //if (Time.time - clickStartTime < 4.8f)
                //{
                //    if (ScreenRecorder.done == true)
                //    {
                //        SecondPanel.SetActive(true);
                //        countdownText.fontSize = 150;
                //        countdownText.text = "OK!";
                //    }
                //}
                //else
                if(Time.time - clickStartTime > 4.2f)
                {
                    countdownObj.SetActive(false);
                    countdownText.fontSize = 300;
                    countdownText.text = "";
                    countdownThread = false;
                    clickStartTime = -1;
                    secondDone = true;
                    secondToThird = true;
                    shotted = false;
                    ScreenRecorder.done = false;
                    //cameraText.text = "";
                }
            }
        }
    }

    void OnClickCorrection(){
        countdownText.text = "3";
        countdownObj.SetActive(true);
        countdownThread = true;
    }
    void OnClickCancellation(){
        SecondPanel.SetActive(false);
    }

    void OnClickGo(){
        ThirdPanel.SetActive(false);
        secondDone = false;
        secondToThird = false;
        SwitchCamera.firstTriggered = true;
    }

    void OnClickBackToCorrect(){
        //SecondPanel.SetActive(true);
        firstDone = true;
        firstToSecond = true;

    }
}
