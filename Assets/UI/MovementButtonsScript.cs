using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The controller for all the manual movement - the arrow keys and touch rotation.
Also toggles live/manual mode.
*/
public class MovementButtonsScript : MonoBehaviour
{
    // Link to the user location and rotation store
    public UserLocationScript loc;
    // Reference to the main virtual camera
    private Camera camera;
    // References to the components for the arrow keys
    public GameObject arrowImage;
    public GameObject forwardButton;
    public GameObject backwardButton;
    public GameObject leftButton;
    public GameObject rightButton;
    // Local store of whether the system is in live mode
    bool liveMode = false;
    // Flags set to true when the corresponding arrow key is pressed
    private bool moveForward = false;
    private bool moveBackward = false;
    private bool moveLeft = false;
    private bool moveRight = false;

    void Start() {
        camera = Camera.main;
    }

    // Updates the user position and rotation in manual mode based on the input
    void Update()
    {
        if (!liveMode) {
            // Touch rotation
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer) {
                HandleTouch();
            } else {
                HandleMouse();
            }
            // Arrow keys
            if (moveForward) { 
                if (!moveBackward) loc.move(0); }
            if (moveBackward) loc.move(180);
            if (moveLeft) {
                if (!moveRight) loc.move(-90); }
            if (moveRight) loc.move(90);

        }
    }

    // Function called by live mode button
    public void toggleLive () {
        liveMode = !liveMode;
        float scale = liveMode ? 0f : 1f;
        arrowImage.transform.localScale = new Vector3(scale,scale,scale);
        loc.LiveMode = liveMode;
    }

    // Functions called by the arrow keys when they are held
    public void enableForward() { moveForward = true; }
    public void disableForward() { moveForward = false; }
    public void enableBackward() { moveBackward = true; }
    public void disableBackward() { moveBackward = false; }
    public void enableLeft() { moveLeft = true; }
    public void disableLeft() { moveLeft = false; }
    public void enableRight() {  moveRight = true; }
    public void disableRight() { moveRight = false; }

    // Handles all touch control of rotation
    // Used tutorial: https://kylewbanks.com/blog/unity3d-panning-and-pinch-to-zoom-camera-with-touch-and-mouse-input
    // https://gist.github.com/seferciogluecce/32c468b4392393f4f394a33a4a3e3c6a
    // Speed at which the virtual camera rotates from touch control 
    float PanSpeed = 100f;
    // The last orientation of the camera
    private Vector3 lastPanPosition;
    // The type of touch
    private int panFingerId;
        
    void HandleTouch() {
        if (Input.touchCount >= 1) {
            // If the touch began, capture its position and its finger ID.
            // Otherwise, if the finger ID of the touch doesn't match, skip it.
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            } else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved) {
                PanCamera(touch.position);
            }
        }
    }
    
    void HandleMouse() {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0)) {
            lastPanPosition = Input.mousePosition;
        } else if (Input.GetMouseButton(0)) {
            PanCamera(Input.mousePosition);
        }
    }

    // Rotates the virtual camera
    void PanCamera(Vector3 newPanPosition) {
        // Determine how much to move the camera
        Vector3 offset = camera.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        loc.rotate(offset.y * PanSpeed, offset.x * PanSpeed);
        // Cache the position
        lastPanPosition = newPanPosition;
    }

}
