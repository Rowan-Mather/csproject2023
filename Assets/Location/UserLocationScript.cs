using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserLocationScript : MonoBehaviour
{
    private GCS userLoc = new GCS();
    public LocationDisplayScript locDisplay;

    void Start()
    {
        locDisplay.updateDisplay(userLoc);
    }

    public void setLocation(double lon, double lat) {
        userLoc.Longitude = lon;
        userLoc.Latitude = lat;
        locDisplay.updateDisplay(userLoc);
    }

    public GCS getLocation() { return userLoc; }


    // Setting the location from the editor for testing
    public double tempLong = 0;
    public double tempLat = 0;
    [ContextMenu("Set location")]
    public void tempLocSet() {
        setLocation(tempLong, tempLat);
    }
}
