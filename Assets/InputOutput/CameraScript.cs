using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Responsible for activating the real devices camera and moving the virtual camera
according to the instructions from the UserLocationScript class. 
*/
public class CameraScript : MonoBehaviour
{
    // Link to the location & rotation class
    public UserLocationScript loc;
    // Link to the IO class
    public InputHandler2Script ioHandler;
    // Flag on status of device's camera
    private bool realCamAvailable = false;
    // Texture that contains the live input from the camera
    private WebCamTexture realCam;
    // Display image made from the texture 
    public RawImage realBackground;
    // Used to adjust the aspect ratio of the projection
    public AspectRatioFitter fit;
    // Plane that creates the floor in manual mode
    public GameObject virtualPlane;
    // Constant scalars - the plane is shown/hidden by scaling it to 1/0
    private Vector3 showVec = new Vector3(1f,1f,1f);
    private Vector3 hideVec = new Vector3(0f,0f,0f);

    void Start() {
        startCamera();
    }

    private void startCamera() {
        // Starts the camera via the I/O hander
        realCam = ioHandler.startCamera();
        if (realCam != null) {
            realCamAvailable = true;
            Debug.Log("Camera available.");
        }
        else {
            Debug.Log("Camera unavailable");
            realCamAvailable = false;
        }
    }

    private void Update()
    {
        // Sets the rotation of the virtual camera to that specified by the 
        // userLocationScript.
        gameObject.transform.rotation = loc.SceneRotation;

        // Uses the live camera input where possible in live mode
        if (loc.LiveMode && realCamAvailable) {
            // Display the input image and hide the plane 
            virtualPlane.transform.localScale = hideVec;
            realBackground.transform.localScale = showVec;
            realBackground.texture = realCam;

            // Use the aspect ratio of the texture to change the transform of 
            // the image appropriately 
            float ratio = (float)realCam.width / (float)realCam.height;
            fit.aspectRatio = ratio;
            float scaleY = realCam.videoVerticallyMirrored ? -1f: 1f;
            realBackground.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
            int orient = -realCam.videoRotationAngle;
            realBackground.rectTransform.localEulerAngles = new Vector3(0,0,orient);
        }
        // In manual mode, simply display the floor plane
        else {
            virtualPlane.transform.localScale = showVec;
            realBackground.transform.localScale = hideVec;
        }
    }

}