using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MirrorCamera : MonoBehaviour
{
    public static bool firstMirror = true;
    void Start()
    {
        firstMirror = true;

    }

    void MirrorFlipCamera(Camera camera)
    {

        Matrix4x4 mat = camera.projectionMatrix;

        mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));

        camera.projectionMatrix = mat;

    }
    void Update(){
        if(SwitchCamera.needMirrored == true && firstMirror == true){
            
            MirrorFlipCamera(this.gameObject.GetComponent<Camera>());
            firstMirror = false;
        
        }
        else if(SwitchCamera.needMirrored == false){
            firstMirror = true;
        }

    }


}