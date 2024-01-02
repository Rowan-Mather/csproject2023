using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyScript : MonoBehaviour
{
    public UserLocationScript loc;
    private bool moveForward = false;
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        if (moveForward) loc.moveForward();
        
    }

    public void EnableForward() { moveForward = true; }
    public void DisableForward() { moveForward = false; }

}
