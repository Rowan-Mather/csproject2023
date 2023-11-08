using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;

using Gyroscope = UnityEngine.InputSystem.Gyroscope;

//using Gyroscope = UnityEngine.InputSystem.Gyroscope;
// ripped off here https://www.youtube.com/watch?v=W70n_bXp7Dc
public class CameraScript : MonoBehaviour
{
    private AttitudeSensor _attitudeSensor;

    void Start() {
        //InputSystem.EnableDevice(Gyroscope.current);
        //InputSystem.EnableDevice(Accelerometer.current);
        
        Debug.Log("hi");
        InputSystem.EnableDevice(AttitudeSensor.current);
        Debug.Log("im dying");
        //InputSystem.EnableDevice(GravitySensor.current);

        _attitudeSensor = InputSystem.GetDevice<AndroidRotationVector>();
        //if (_attitudeSensor == null)
        //    _attitudeSensor = InputSystem.GetDevice<AndroidGameRotationVector>();

        if (_attitudeSensor != null)
            InputSystem.EnableDevice(_attitudeSensor);
        else Debug.Log("i hate my life");
    }
     void Update() {
        //Vector3 angularVelocity = Gyroscope.current.angularVelocity.ReadValue();
        //Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
        //Vector3 attitude = AttitudeSensor.current.attitude.ReadValue(); // ReadValue() returns a Quaternion
        //Vector3 gravity = GravitySensor.current.gravity.ReadValue();

        // transform.rotation = _attitudeSensor.attitude.ReadValue();
        
        //  AttitudeSensor.current.attitude.ReadValue();
        

        /*
        text.text = $"Angular Velocity\nX={angularVelocity.x:#0.00} Y={angularVelocity.y:#0.00} Z={angularVelocity.z:#0.00}\n\n" +
                        $"Acceleration\nX={acceleration.x:#0.00} Y={acceleration.y:#0.00} Z={acceleration.z:#0.00}\n\n" +
                            $"Attitude\nX={attitude.x:#0.00} Y={attitude.y:#0.00} Z={attitude.z:#0.00}\n\n" +
                             $"Gravity\nX={gravity.x:#0.00} Y={gravity.y:#0.00} Z={gravity.z:#0.00}";
        */
    }
    /*
    public UserLocationScript userLocation;
    private float speed = 100;
    public float sensitivity = 5.0f;
    private AttitudeSensor _attitudeSensor;
    private IEnumerator Start()
    {
        _attitudeSensor = InputSystem.GetDevice<AndroidRotationVector>();
        if (_attitudeSensor == null)
            _attitudeSensor = InputSystem.GetDevice<AndroidGameRotationVector>();

        if (_attitudeSensor != null)
            InputSystem.EnableDevice(_attitudeSensor);
        
        //https://issuetracker.unity3d.com/issues/android-gyroscope-functionality-doesnt-respond-in-the-player-when-using-a-tablet
    }

    private void ApplyGyroRotation()
    {
        //Quaternion gyroAttitude = Input.gyro.attitude;
        Quaternion gyroAttitude = Quaternion.identity;
        if (_attitudeSensor != null)
            gyroAttitude = _attitudeSensor.attitude.ReadValue();
    } 

    //void Start() {
    //    Input.gyro.enabled = false;
    //    Input.gyro.enabled = true;
    //}
    void Update()
    {

        if (_attitudeSensor != null)
            transform.rotation = _attitudeSensor.attitude.ReadValue();

        //transform.rotation = userLocation.SceneRotation;


        //userLocation.SceneRotation;
        
        // Move the camera forward, backward, left, and right
        //transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        //float a = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //float b = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // Rotate the camera based on the mouse movement
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");
        //transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
        
        //transform.eulerAngles += new Vector3(-a, b, 0);
        //Quaternion.Euler(-v, h, 0);
        
    }*/
}