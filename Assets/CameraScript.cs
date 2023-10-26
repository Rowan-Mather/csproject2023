using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ripped off here https://www.youtube.com/watch?v=W70n_bXp7Dc
public class CameraScript : MonoBehaviour
{
    private float speed = 100;
    public float sensitivity = 5.0f;

    void Update()
    {
        // Move the camera forward, backward, left, and right
        //transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        float a = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float b = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // Rotate the camera based on the mouse movement
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");
        //transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
        transform.eulerAngles += new Vector3(-a, b, 0);

/*
        var v3 = new Vector3(
            Input.GetAxis("Vertical") * speed * Time.deltaTime, 
            Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0.0);
        transform.Rotate(v3); 

        */
    }
}
    /*
        Vector3 velocity;
    
    private void KeyboardInput()
    {
            var h_Input = Input.GetAxis("Horizontal");
            var v_Input = Input.GetAxis("Vertical");
    
            Vector3 translation = v_Input * transform.up;
            translation += h_Input * transform.right;
            translation.z = 0;
    
            if (translation.magnitude > 0)
            {
                velocity = translation;
            }
            else
            {
                velocity = Vector3.zero;
            }
    
            if (velocity.magnitude > 0)
            {
                   var lookRotate = Quaternion.LookRotation(velocity);
                   transform.rotation = Quaternion.Slerp(transform.rotation, lookRotate, Time.deltaTime * rotationSpeed);
            }
    }
}
*/