using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=JWccDbm69Cg used this for basic gps location
// rotation https://stackoverflow.com/questions/42141056/how-to-detect-android-phone-rotation-in-unity
public class UserLocationScript : MonoBehaviour
{
    private GCS userLoc = new GCS();
    public LocationDisplayScript locDisplay;
    private Gyroscope gyro;
    private Quaternion sceneRotation = new Quaternion(0,0,0,0);
    private bool livePossible = false;
    public bool liveMode;
    private float manualSpeed = 100;
    void Start()
    {
        locDisplay.updateDisplay(userLoc);
        if (!Input.location.isEnabledByUser || !SystemInfo.supportsGyroscope)
        {
            if (!Input.location.isEnabledByUser)
                Debug.LogError("Location services not enabled on device");
            if (!SystemInfo.supportsGyroscope)
                Debug.LogError("Device does not support gyroscope.");
            return;
        }
        // Start location services
        Input.location.Start();
        // Set the desired accuracy for GPS data to 1m
        Input.location.Start(1f, 1f);
        // Initialise the gyroscope
        gyro = Input.gyro;
        gyro.enabled = true;
        livePossible = true;
    }

    private void Update() {
        // maybe set the altitude much less frequently than the longitude & lat since it should change less generally
        // Check if GPS data is available & live mode is enabled
        if (livePossible & liveMode) {
            //if (Input.location.status == LocationServiceStatus.Running)
            setLocation(
                Input.location.lastData.longitude, 
                Input.location.lastData.latitude, 
                Input.location.lastData.altitude);
            sceneRotation = GyroToUnity(gyro.attitude);            
        }
        else {
            float leftright = Input.GetAxis("Vertical") * manualSpeed * Time.deltaTime;
            float updown = Input.GetAxis("Horizontal") * manualSpeed * Time.deltaTime;
        }
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return Quaternion.Euler(90, 0, 0) * new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    
    public void setLocation(double lon, double lat, double alt) {
        userLoc.Longitude = lon;
        userLoc.Latitude = lat;
        userLoc.Altitude = alt;
        locDisplay.updateDisplay(userLoc);
    }

    public GCS getLocation() { return userLoc; }
    public Quaternion getRotation() { return sceneRotation; }

    // Setting the location from the editor for testing
    public double tempLong = 0;
    public double tempLat = 0;
    public double tempAlt = 0;
    [ContextMenu("Set location")]
    public void tempLocSet() {
        setLocation(tempLong, tempLat, tempAlt);
    }
}