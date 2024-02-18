using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public UserLocationScript loc;
    private IOHandler device;
    private bool realCamAvailable = false;
    private WebCamTexture realCam;
    //private Texture defaultBackground;
    public GameObject virtualPlane;
    public RawImage realBackground;
    public AspectRatioFitter fit;

    private Vector3 showVec = new Vector3(1f,1f,1f);
    private Vector3 hideVec = new Vector3(0f,0f,0f);

    void Start() {
        //while (device == null) 
        startCamera();

    }

    private void startCamera() {
        device = loc.getIO();
        if (device == null) return;
        realCam = device.startCamera();
        if (realCam != null) {
            realCamAvailable = true;
            Debug.Log("Camera available.");
        }
        else {
            Debug.Log("Camera unavailable");
            realCamAvailable = false;
        }
    }

    float X;
    float Y;
    float speed = 5;

    private void Update()
    {
        if (device == null) {
            startCamera();
        }

        //gameObject.transform.position = loc.getLocation();
        gameObject.transform.rotation = loc.SceneRotation;

        if (loc.LiveMode && realCamAvailable) {
            // grab the camera input
            virtualPlane.transform.localScale = hideVec;
            realBackground.transform.localScale = showVec;
            realBackground.texture = realCam;

            // Update camera angles
            float ratio = (float)realCam.width / (float)realCam.height;
            fit.aspectRatio = ratio;
            float scaleY = realCam.videoVerticallyMirrored ? -1f: 1f;
            realBackground.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
            int orient = -realCam.videoRotationAngle;
            realBackground.rectTransform.localEulerAngles = new Vector3(0,0,orient);
        }
        else {
            virtualPlane.transform.localScale = showVec;
            realBackground.transform.localScale = hideVec;
            //plane.show();
        }
    }

}