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
    private bool pointerHeld = false;
    private Vector3 pointerOrigin;
    // Position
    public GameObject forwardButton;
    private bool moveForward = false;

    void Start() {
        pointerOrigin = ball.transform.position;
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
                float cap = Mathf.Min(35, dist);
                float capCos = Mathf.Cos(dir) * cap;
                float capSin = Mathf.Sin(dir) * cap;
                pointer.transform.position = new Vector3(
                    pointerOrigin.x + capCos, 
                    pointerOrigin.y + capSin,
                    0);

                loc.rotate(-0.01f*capSin, 0.01f*capCos);
            }
            else {
                pointer.transform.position = pointerOrigin;
            }
        }
    }

    public void enableForward() { moveForward = true; }
    public void disableForward() { moveForward = false; }

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
