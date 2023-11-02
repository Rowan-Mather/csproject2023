using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=JWccDbm69Cg used this for basic gps location
public class UserLocationScript : MonoBehaviour
{
    private GCS userLoc = new GCS();
    //private GCS lastUserLoc = new GCS();
    public LocationDisplayScript locDisplay;
    //private float timeInterval = 0f;

    void Start()
    {
        locDisplay.updateDisplay(userLoc);
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services not enabled on device");
            return;
        }
        // Start location services
        Input.location.Start();
        // Set the desired accuracy for GPS data
        Input.location.Start(1f, 1f);
        // Store the current time to calculate the time interval later
        //timeInterval = Time.time;
    }

    private void Update() {
        // Check if GPS data is available
        if (Input.location.status == LocationServiceStatus.Running)
        {
            setLocation(
                Input.location.lastData.longitude, 
                Input.location.lastData.latitude, 
                Input.location.lastData.altitude);
        }
    }
    
    //maybe set the altitude much less frequently than the longitude & lat since it should change less generally
    public void setLocation(double lon, double lat, double alt) {
        userLoc.Longitude = lon;
        userLoc.Latitude = lat;
        userLoc.Altitude = alt;
        locDisplay.updateDisplay(userLoc);
    }

    public GCS getLocation() { return userLoc; }


    // Setting the location from the editor for testing
    public double tempLong = 0;
    public double tempLat = 0;
    public double tempAlt = 0;
    [ContextMenu("Set location")]
    public void tempLocSet() {
        setLocation(tempLong, tempLat, tempAlt);
    }
}