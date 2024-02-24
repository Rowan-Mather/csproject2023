using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class IOHandler
{
    private int gyroAttempts = 0;
    private int gpsAttempts = 0;

    private Gyroscope gyro = null;

    // Enables a new unity input system device.
    private static void EnableDeviceIfNeeded(InputDevice device)
    {
        if (device != null && !device.enabled)
            InputSystem.EnableDevice(device);
    }

    // Attempts to find an input sensor of the specified type and returns it.
    private static TDevice GetRemoteDevice<TDevice>()
        where TDevice : InputDevice
    {
        foreach (var device in InputSystem.devices)
            if (device.remote && device is TDevice deviceOfType)
                return deviceOfType;
        return default;
    }

    // Attempts to get the current gyroscope input. If there are more than 5
    // unsuccessful attempts made, the empty vector will always be returned.
    public Vector3 getGyro() {
        if (gyro == null) gyro = GetRemoteDevice<Gyroscope>();
        //if (gyroAttempts > 5) return new Vector3(0, 0, 0);
        //var gyro = GetRemoteDevice<Gyroscope>();
        //var attitude = GetRemoteDevice<AttitudeSensor>();
        //var acceleration = GetRemoteDevice<LinearAccelerationSensor>();
        //EnableDeviceIfNeeded(attitude);
        if (gyro != null) {
            EnableDeviceIfNeeded(gyro);
            var rot = gyro.angularVelocity.ReadValue();
            return new Vector3(-round(rot.x), -round(rot.y), 0);
        }
        else {
            // Todo: change this to an impossible angle
            Debug.LogError("Gyroscope unavailable.");
            gyroAttempts ++;
            return new Vector3(0, 0, 0);
        }
    }

    // Rounds a double to 2dp in float.
    private float round(float val) {
        return Mathf.Round(val * 100) / 100;
    }

    // Requests the approximate location [permission from the user device
    private bool coarseLocationPerm() {
        if (Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)) {
            Debug.Log("Coarse location permission granted.");
            return true;
        }
        else
        {
            Debug.Log("Requesting coarse location permission.");
            Permission.RequestUserPermission(Permission.CoarseLocation);
            new WaitForSeconds(10);
            if (Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)) {
                Debug.Log("Coarse location permission granted.");
                return true;
            }
            return false;
        }
    }

    // Requests the exact location permission from the user device.
    private bool fineLocationPerm() {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
            Debug.Log("Fine location permission granted.");
            return true;
        }
        else
        {
            Debug.Log("Requesting fine location permission.");
            Permission.RequestUserPermission(Permission.FineLocation);
            new WaitForSeconds(10);
            if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
                Debug.Log("Fine location permission granted.");
                return true;
            }
            return false;
        }
    }

    // Gets the users current device location if possible and returns it as a 
    // GCS co-ordinate. If more than 5 attempts are made unsuccessfully, the 
    // function will start always returning null.
    public GCS getLocation() {
        if (gpsAttempts > 5) return null;
        // Gets the location.
        if (Input.location.status == LocationServiceStatus.Running) {
            var loc = new GCS(
                Input.location.lastData.longitude,
                Input.location.lastData.latitude, 
                Input.location.lastData.altitude
            );
            return loc;
        }
        // Waits on the initialising location service, then tries to get it.
        else if (Input.location.status == LocationServiceStatus.Initializing) {
           new WaitForSeconds(8);
           if (Input.location.status == LocationServiceStatus.Running) {
                var loc = new GCS(
                    Input.location.lastData.longitude,
                    Input.location.lastData.latitude, 
                    Input.location.lastData.altitude
                );
                return loc;
           }
           gpsAttempts++;
           return null;
        }
        // If the location service is not running, returns null.
        else {
            gpsAttempts++;
            return null;
        }
    }

    // Starts the tracking of the user device GPS location if possible.
    // Code modified from: https://docs.unity3d.com/ScriptReference/LocationService.Start.html
    public IEnumerator StartLocation()
    {
        Debug.Log("Starting location...");
        if ( !coarseLocationPerm() || !fineLocationPerm() ) {
            yield break;
        }
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start(1f, 1f);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
    }

    // Starts the importing of the device camera live data. Returns a texture
    // with the input.
    // Once the camera is started, continuous data is queried directly from the 
    // virtual camera.
    // Based on code from this tutorial: https://www.youtube.com/watch?v=c6NXkZWXHnc
    public WebCamTexture startCamera() {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0) {
            // No camera available
            return null;
        }
        else {
            for (int i = 0; i < devices.Length; i++) {
                if (!devices[i].isFrontFacing) {
                    // Found a camera on the back of the device
                    WebCamTexture backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                    backCam.Play();
                    return backCam;
                }
            }
            //No backcam but frontcam
            WebCamTexture otherCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
            otherCam.Play();
            return otherCam;	
        }
        return null;
    }
}
