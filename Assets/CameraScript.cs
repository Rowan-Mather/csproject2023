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
    }
}