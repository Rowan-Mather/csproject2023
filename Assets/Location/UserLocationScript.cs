using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Central controller for all user movement - location AND rotation. Stores the 
current values and has functions to move around. Also updates the location
display.
*/
// used this tutorial for basic gps location https://www.youtube.com/watch?v=JWccDbm69Cg
// rotation https://stackoverflow.com/questions/42141056/how-to-detect-android-phone-rotation-in-unity
public class UserLocationScript : MonoBehaviour
{
    // Link to the I/O handler script
    public InputHandler2Script ioHandler;
    // Link to the script for displaying the current GPS location
    public LocationDisplayScript locDisplay;

    // Current location as a GCS (GPS & altitude) co-ordinate
    private GCS userLoc = new GCS();
    // Current orientation as a quarternion
    private Quaternion sceneRotation = new Quaternion(0,0,0,0);
    public Quaternion SceneRotation { get { return sceneRotation; } }
    // Flag as to whether the user is in manual or live mode for movement
    private bool liveMode = false;
    public bool LiveMode {
        get { return liveMode; }
        set { liveMode = value; }
    }
    // Speed at which the user moves through the world in manual mode, 
    // calculated using the number of metres to a degree of latitude and 
    // equating to approximately 5m/s
    private const double manualSpeed = 1f/111139f * 5;

    void Start()
    {
        locDisplay.updateLocationDisplay(userLoc);
        setLiveLocation();
    }

    int livestarter = 0; 
    private void Update() {
        if (liveMode) {
            setLiveLocation();
            setLiveRotation();
        }
        if (livestarter < 300) {
            setLiveLocation();
            livestarter++;
        }

    }

    // --------- LOCATION --------- //

    // Returns the GCS variable corresponding to the currend location
    public GCS getLocation() { return userLoc; }

    // Sets location manually and updates the display
    public void setLocation(double lon, double lat, double alt) {
        userLoc.Longitude = lon;
        userLoc.Latitude = lat;
        userLoc.Altitude = 0; //alt;
        locDisplay.updateLocationDisplay(userLoc);
    }

    // Adjusts location manually by a fixed amount in the direction specified by
    // the angle and updates the display
    public void move(int angle = 0) {
        var rot = sceneRotation.eulerAngles.y + angle;
        userLoc.Longitude -= Mathf.Cos(Mathf.Deg2Rad * rot) * manualSpeed * Time.deltaTime;
        userLoc.Latitude -= Mathf.Sin(Mathf.Deg2Rad * rot) * manualSpeed * Time.deltaTime;
        locDisplay.updateLocationDisplay(userLoc);
    }

    // Sets location in live mode accordig to the GPS readings
    public void setLiveLocation() {
        GCS gps = ioHandler.getLocation();
        if (gps != null) {
            //To show: Debug.Log("Lon: "+ gps.Longitude.ToString() + " Lat: "+ gps.Latitude);
            userLoc.Longitude = gps.Longitude;
            userLoc.Latitude = gps.Latitude;
            userLoc.Altitude = 0; //gps.Altitude;
            locDisplay.updateLocationDisplay(gps);
        }
    }

    /*
    // Manually setting the location from the editor for testing
    public double tempLong = 0;
    public double tempLat = 0;
    public double tempAlt = 0;
    [ContextMenu("Set location")]
    public void tempLocSet() {
        setLocation(tempLong, tempLat, tempAlt);
    }
    */

    // --------- ROTATION --------- //

    // Adjust rotation manually along the x and y axes (up/down, left/right)
    public void rotate(float x, float y) {
        sceneRotation.eulerAngles += new Vector3(x,y,0);
        ioHandler.setGyro(sceneRotation.eulerAngles);
    }

    // Set orientation to the relative one calculated from the gyroscope
    public void setLiveRotation() {
        sceneRotation.eulerAngles = ioHandler.getGyro();
    }
}