using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=JWccDbm69Cg used this for basic gps location
// rotation https://stackoverflow.com/questions/42141056/how-to-detect-android-phone-rotation-in-unity
public class UserLocationScript : MonoBehaviour
{
    private GCS userLoc = new GCS();
    private IOHandler device;
    public LocationDisplayScript locDisplay;
    private Quaternion sceneRotation = new Quaternion(0,0,0,0);
    public Quaternion SceneRotation { get { return sceneRotation; } }
    private double manualSpeed = 0.1;
    private bool liveMode = false;
    public bool LiveMode {
        get { return liveMode; }
        set { liveMode = value; }
    }

    void Start()
    {
        device = new IOHandler();
        StartCoroutine(device.StartLocation());
        locDisplay.updateDisplay(userLoc);

        /*
        if (UnityEngine.InputSystem.Gyroscope.current != null) {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }
        if (AttitudeSensor.current != null) {
                InputSystem.EnableDevice(AttitudeSensor.current);
        }*/
        /*
        // Live location
        if (Input.location.isEnabledByUser) {
            // Start location services
            Input.location.Start();
            // Set the desired accuracy for GPS data to 1m
            Input.location.Start(1f, 1f);
            liveLocation = true;
            Debug.LogError("Location enabled.");
        } else {
            Debug.LogError("Location services not enabled on device.");
        }
        // Live orientation
        if (SystemInfo.supportsGyroscope)
        {
            // Initialise the gyroscope
            //Input.gyro.enabled = true;
            //myGyro = Input.gyro;
            liveRotation = false;
            Debug.LogError("Rotation enabled.");

        } else {
            Debug.LogError("Device does not support gyroscope.");
        }
        if (liveLocation && liveRotation) liveMode = true;
        */
    }

    private void Update() {
        if (liveMode) {
            setLiveLocation();
            setLiveRotation();
        }

        /*
        if (liveMode) {
            if (liveLocation) {
                setLocation(
                    Input.location.lastData.longitude, 
                    Input.location.lastData.latitude, 
                    Input.location.lastData.altitude
                );
            }
        }
        if (liveRotation) {
            sceneRotation = GyroToUnity(Input.gyro.attitude); 
        } else {
            float leftright = Input.GetAxis("Vertical") * manualSpeed * Time.deltaTime;
            float updown = Input.GetAxis("Horizontal") * manualSpeed * Time.deltaTime;
            sceneRotation.eulerAngles += new Vector3(-leftright, updown, 0);
            //Quaternion.Euler(-leftright, updown, 0);
        }
        */
        // note: maybe set the altitude much less frequently than the longitude & lat since it should change less generally
    }

    /*private static Quaternion GyroToUnity(Quaternion q)
    {
        return Quaternion.Euler(90, 0, 0) * new Quaternion(q.x, q.y, -q.z, -q.w);
    }*/

    public GCS getLocation() { return userLoc; }

    // Manual location set
    public void setLocation(double lon, double lat, double alt) {
        userLoc.Longitude = lon;
        userLoc.Latitude = lat;
        userLoc.Altitude = alt;
        locDisplay.updateDisplay(userLoc);
    }

    // Manual location adjust
    public void moveForward() {
        var rot = sceneRotation.eulerAngles.y;
        userLoc.Longitude -= Mathf.Cos(Mathf.Deg2Rad * rot) * manualSpeed;
        userLoc.Latitude -= Mathf.Sin(Mathf.Deg2Rad * rot) * manualSpeed;
        locDisplay.updateDisplay(userLoc);
    }

    public void setLiveLocation() {
        GCS gps = device.getLocation();
        if (gps != null) {
            Debug.Log("Lon: "+ gps.Longitude.ToString() + " Lat: "+ gps.Latitude);
            //userLoc.Longitude = gps.Longitude;
            //userLoc.Latitude = gps.Latitude;
            //userLoc.Altitude = gps.Altitude;
            locDisplay.updateDisplay(gps);
        }
    }

    // Manual rotation adjust
    public void rotate(float x, float y) {
        sceneRotation.eulerAngles += new Vector3(x,y,0);
    }

    public void setLiveRotation() {
        Vector3 rotChange = device.getGyro();
        if (rotChange != null) {
            sceneRotation.eulerAngles += device.getGyro();
        }
    }

    //public Quaternion getRotation() { return sceneRotation; }

    // Setting the location from the editor for testing
    public double tempLong = 0;
    public double tempLat = 0;
    public double tempAlt = 0;
    [ContextMenu("Set location")]
    public void tempLocSet() {
        setLocation(tempLong, tempLat, tempAlt);
    }
}