using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSphere : MonoBehaviour
{
    public UserLocationScript loc;
    public GameObject pointer;
    public GameObject ball;
    private bool held = false;
    private Vector3 pointerOrigin;
    // Start is called before the first frame update
    void Start()
    {
        pointerOrigin = ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (held) {
            Vector3 direction = Input.mousePosition - pointerOrigin;
            Vector3 unit = new Vector3(1,0,0);
            float dir = Vector3.Angle(direction, unit);
            if (Input.mousePosition.y-pointerOrigin.y < 0) dir = 360-dir;
            dir *= Mathf.Deg2Rad;

            float dist = Vector3.Distance(pointerOrigin, Input.mousePosition);
            float cap = Mathf.Min(25, dist);
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

    public void movePointer() {
        held = true;
    }

    public void reset () {
        held = false;
    }
}
