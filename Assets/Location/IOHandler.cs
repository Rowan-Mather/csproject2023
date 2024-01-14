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

    public Vector3 getGyro() {
        if (gyroAttempts > 5) return new Vector3(0, 0, 0);
        var gyro = GetRemoteDevice<Gyroscope>();
        //var attitude = GetRemoteDevice<AttitudeSensor>();
        //var acceleration = GetRemoteDevice<LinearAccelerationSensor>();
        //EnableDeviceIfNeeded(attitude);
        if (gyro != null) {
            EnableDeviceIfNeeded(gyro);
            var rot = gyro.angularVelocity.ReadValue();
            //return new Vector3(-round(rot.x), -round(rot.y), -round(rot.z));
            return new Vector3(-round(rot.x), -round(rot.y), 0);
        }
        else {
            // todo change this to an imposiblle angle output but print angle to work out caps
            Debug.LogError("Gyroscope unavailable.");
            gyroAttempts ++;
            return new Vector3(0, 0, 0);
        }
        //if (attitude != null) {        
        //    if (attitude.attitude.ReadValue().eulerAngles != new Vector3(0,0,0)) {
        //        Debug.Log(attitude.attitude.ReadValue());
        //    }
        //}
        //if (acceleration != null) {        
        //    if (acceleration.acceleration.ReadValue() != new Vector3(0,0,0)) {
        //        Debug.Log(acceleration.acceleration.ReadValue());
        //    }
        //}
    }

    private float round(float val) {
        return Mathf.Round(val * 100) / 100;
    }

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

    // https://docs.unity3d.com/ScriptReference/LocationService.Start.html
    public GCS getLocation() {
        if (gpsAttempts > 5) return null;
        if (Input.location.status == LocationServiceStatus.Running) {
            var loc = new GCS(
                Input.location.lastData.longitude,
                Input.location.lastData.latitude, 
                Input.location.lastData.altitude
            );
            return loc;
        }
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
        else {
            gpsAttempts++;
            return null;
        }
    }

    /*

        if ( !coarseLocationPerm() || !fineLocationPerm() ) {
            return;
        }
        Input.location.Start(1f, 1f);
        if (Input.location.isEnabledByUser == true) {
            Debug.Log("Location enabled by user.");
        }
        else {
            Debug.Log("Location not enabled by user.");
        }
        new WaitForSeconds(5); // Wait for location services.
        switch (Input.location.status) {
            case LocationServiceStatus.Failed:
                Debug.Log("LocationServiceStatus is on status Failed");
                break;
            case LocationServiceStatus.Stopped:
                Debug.Log("LocationServiceStatus is on status Stopped");
                break;
            case LocationServiceStatus.Initializing:
                Debug.Log("LocationServiceStatus is on status Initializing");
                break;
            case LocationServiceStatus.Running:
                Debug.Log("LocationServiceStatus is on status Running");
                break;
            default :
                Debug.Log("LocationServiceStatus has no known status");
                break;
        }
        return;
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location.");
            return;
        }
        Debug.Log(
            "Location: " + Input.location.lastData.latitude + " " + 
            Input.location.lastData.longitude + " " + 
            Input.location.lastData.altitude);
            */

        /*
         while (!UnityEditor.EditorApplication.isRemoteConnected)
 {
     yield return null;
 }
        */
        /* Debug.Log(
            "Location: " + Input.location.lastData.latitude + " " + 
            Input.location.lastData.longitude + " " + 
            Input.location.lastData.altitude + " " + 
            Input.location.lastData.horizontalAccuracy + " " + 
            Input.location.lastData.timestamp);*/



        /*
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.Log("Location not enabled on device or app does not have permission to access location.");

        // Starts the location service.
        Input.location.Start(1f, 1f);

        // Waits until the location service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            return;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            return;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
        // if (Input.location.isEnabledByUser) {
        //     // Start location services
        //     Input.location.Start();
        //     // Set the desired accuracy for GPS data to 1m
        //     Input.location.Start(1f, 1f);
        //     //liveLocation = true;
        //     Debug.LogError("Location enabled.");
        // } else {
        //     Debug.LogError("Location services not enabled on device.");
        // }
        */


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

        // Stops the location service if there is no need to query location updates continuously.
        //Input.location.Stop();
    }



    // Camera tutorial
    // https://www.youtube.com/watch?v=c6NXkZWXHnc
    /*private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;*/
    public WebCamTexture startCamera() {
        //defaultBackground = background.texture;
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
