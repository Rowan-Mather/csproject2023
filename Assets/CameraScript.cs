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
    public UserLocationScript loc;
    //private AttitudeSensor _attitudeSensor;

    void Start() {}

    private void Update()
    {
        //gameObject.transform.position = loc.getLocation();
        gameObject.transform.rotation = loc.SceneRotation;
        //UpdateGyro();
    }    

    private void UpdateGyro() {
        var gyro = GetRemoteDevice<Gyroscope>();
        var attitude = GetRemoteDevice<AttitudeSensor>();
        var acceleration = GetRemoteDevice<LinearAccelerationSensor>();
        EnableDeviceIfNeeded(gyro);
        EnableDeviceIfNeeded(attitude);
        if (gyro != null) {
            var rot = gyro.angularVelocity.ReadValue();
            gameObject.transform.eulerAngles += new Vector3(-rot.x, -rot.y, -rot.z);
        }
        if (attitude != null) {        
            if (attitude.attitude.ReadValue().eulerAngles != new Vector3(0,0,0)) {
                Debug.Log(attitude.attitude.ReadValue());
            }
        }
        if (acceleration != null) {        
            if (acceleration.acceleration.ReadValue() != new Vector3(0,0,0)) {
                Debug.Log(acceleration.acceleration.ReadValue());
            }
        }
        
        /*
        var gyro = GetRemoteDevice<Gyroscope>();
        //var attitude = GetRemoteDevice<AttitudeSensor>();
        //var gravity = GetRemoteDevice<GravitySensor>();
        //var acceleration = GetRemoteDevice<LinearAccelerationSensor>();

        // Enable gyro from remote, if needed.
        EnableDeviceIfNeeded(gyro);
        //EnableDeviceIfNeeded(attitude);
        //EnableDeviceIfNeeded(gravity);
        //EnableDeviceIfNeeded(acceleration);

        string text;
        if (gyro == null && attitude == null && gravity == null && acceleration == null)
        {
            text = "No remote gyro found.";
        }
        else
        {
            string gyroText = null;
            string attitudeText = null;
            string gravityText = null;
            string accelerationText = null;

            if (gyro != null)
            {
                var rotation = gyro.angularVelocity.ReadValue();
                gyroText = $"Rotation: x={rotation.x} y={rotation.y} z={rotation.z}";

                // Update rotation of cube.
                //m_Rotation += rotation;
                //rotatingCube.localEulerAngles = m_Rotation;
            }

            if (attitude != null)
            {
                var attitudeValue = attitude.attitude.ReadValue();
                attitudeText = $"Attitude: x={attitudeValue.x} y={attitudeValue.y} z={attitudeValue.z} w={attitudeValue.w}";
            }

            if (gravity != null)
            {
                var gravityValue = gravity.gravity.ReadValue();
                gravityText = $"Gravity: x={gravityValue.x} y={gravityValue.y} z={gravityValue.z}";
            }

            if (acceleration != null)
            {
                var accelerationValue = acceleration.acceleration.ReadValue();
                accelerationText = $"Acceleration: x={accelerationValue.x} y={accelerationValue.y} z={accelerationValue.z}";
            }

            text = string.Join("\n", gyroText, attitudeText, gravityText, accelerationText);
            Debug.Log(text);
        }

        //gyroInputText.text = text;
        */
    }

   private static void EnableDeviceIfNeeded(InputDevice device)
    {
        if (device != null && !device.enabled)
            InputSystem.EnableDevice(device);
    }

    // Make sure we're not thrown off track by locally having sensors on the device. Instead
    // explicitly grab the remote ones.
    private static TDevice GetRemoteDevice<TDevice>()
        where TDevice : InputDevice
    {
        foreach (var device in InputSystem.devices)
            if (device.remote && device is TDevice deviceOfType)
                return deviceOfType;
        return default;
    }

}