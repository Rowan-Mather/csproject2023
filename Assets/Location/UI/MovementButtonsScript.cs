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
    public GameObject forwardButton;
    private bool moveForward = false;

    void Start() {
        camera = Camera.main;
        pointerOrigin.x = ball.transform.position.x;
        pointerOrigin.y = ball.transform.position.y;
        pointerOrigin.z = ball.transform.position.z;
        pointerHeld = false;
    }

    void Update()
    {
        if (!liveMode) {
            if (moveForward) loc.moveForward();
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
            }
        }
    }

    public void enableForward() { moveForward = true; }
    public void disableForward() { moveForward = false; }

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

    public void toggleLive () {
        liveMode = !liveMode;
        float scale = liveMode ? 0f : 1f;
        pointer.transform.localScale = new Vector3(scale,scale,scale);
        ball.transform.localScale = new Vector3(scale,scale,scale);
        forwardButton.transform.localScale = new Vector3(scale,scale,scale);
        loc.LiveMode = liveMode;
    }

}
