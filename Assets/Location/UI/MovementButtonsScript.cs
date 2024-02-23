using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButtonsScript : MonoBehaviour
{
    public UserLocationScript loc;
    bool liveMode = false;
    // Rotation
    public GameObject pointer;
    public GameObject ball;
    private Camera camera;
    private bool pointerHeld = false;
    private Vector3 pointerOrigin = new Vector3();
    // Position
    public GameObject arrowImage;
    public GameObject forwardButton;
    public GameObject backwardButton;
    public GameObject leftButton;
    public GameObject rightButton;
    private bool moveForward = false;
    private bool moveBackward = false;
    private bool moveLeft = false;
    private bool moveRight = false;

    void Start() {
        camera = Camera.main;
        pointerOrigin.x = ball.transform.position.x;
        pointerOrigin.y = ball.transform.position.y;
        pointerOrigin.z = ball.transform.position.z;
        pointerHeld = false;
    }

    //int rotationSpeed = 5;

    void Update()
    {
        //if (!liveMode) {
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

            
            
            /*if (Input.GetMouseButton(0)) {
                loc.rotate(
                    -rotationSpeed*Input.GetAxis("Mouse Y"), 
                    rotationSpeed*Input.GetAxis("Mouse X")
                );
                //transform.Rotate(transform.up ,-Input.GetAxis("Mouse X") * Speed  ); //1
            }*/
            /*
            if (pointerHeld) {
                Vector3 direction = Input.mousePosition - pointerOrigin;
                Vector3 unit = new Vector3(1,0,0);
                float dir = Vector3.Angle(direction, unit);
                if (Input.mousePosition.y-pointerOrigin.y < 0) dir = 360-dir;
                dir *= Mathf.Deg2Rad;

                float dist = Vector3.Distance(pointerOrigin, Input.mousePosition);
                float cap = Mathf.Min(80, dist);
                float capCos = Mathf.Cos(dir) * cap;
                float capSin = Mathf.Sin(dir) * cap;
                pointer.transform.position = new Vector3(
                    pointerOrigin.x + capCos, 
                    pointerOrigin.y + capSin,
                    0);
                float tScale = Time.deltaTime;
                loc.rotate(-tScale*capSin, tScale*capCos);
            }
            else {
                pointer.transform.position = pointerOrigin;
            }*/
        //}
    }

    public void enableForward() { moveForward = true; }
    public void disableForward() { moveForward = false; }
    public void enableBackward() { moveBackward = true; }
    public void disableBackward() { moveBackward = false; }
    public void enableLeft() { moveLeft = true; }
    public void disableLeft() { moveLeft = false; }
    public void enableRight() {  moveRight = true; }
    public void disableRight() { moveRight = false; }

    // private bool clicking = false;
    // private bool hovering = false;
    // public void OnMouseDown() {
    //     clicking = true;
    // }

    // public void OnMouseUp() {
    //     clicking = false;
    // }

    // public void OnMouseEnter() {
    //     hovering = true;
    // }

    // public void OnMouseExit() {
    //     hovering = false;
    // }

    public void movePointer() { pointerHeld = true; }

    public void resetPointer () { pointerHeld = false; }

    //https://kylewbanks.com/blog/unity3d-panning-and-pinch-to-zoom-camera-with-touch-and-mouse-input
    //https://gist.github.com/seferciogluecce/32c468b4392393f4f394a33a4a3e3c6a
    float PanSpeed = 100f;
    
    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only
        
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
    
    void PanCamera(Vector3 newPanPosition) {
        // Determine how much to move the camera
        Vector3 offset = camera.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        loc.rotate(offset.y * PanSpeed, offset.x * PanSpeed);
    
        // Cache the position
        lastPanPosition = newPanPosition;
    }

    public void toggleLive () {
        liveMode = !liveMode;
        float scale = liveMode ? 0f : 1f;
        //pointer.transform.localScale = new Vector3(scale,scale,scale);
        //ball.transform.localScale = new Vector3(scale,scale,scale);
        arrowImage.transform.localScale = new Vector3(scale,scale,scale);
        //forwardButton.transform.localScale = new Vector3(scale,scale,scale);
        loc.LiveMode = liveMode;
    }

}
