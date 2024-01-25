using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoricalSiteScript : MonoBehaviour
{
    /* Available Models (with tags) */
    // These can optionally have a particular time period allocated.
    public GameObject timeComponentTemplate;
    List<GameObject> timeComponents = new List<GameObject>();

    /* Location */
    // Approximately one latitude degree in metres.
    private const double GCSscaler = 111139;
    // The distance in degrees to which the model is rendered (0.01), squared.
    private const double sceneRange = 0.0001;
    private GCS GCSlocation = new GCS();
    private float sceneX = 0;
    private float sceneY = 0;
    private float sceneZ = 0;
    bool inRange = false;

    // Add one of the time component prefab instances.
    public GameObject addEmptyTimeComponent() {
        GameObject timeComponent = Instantiate(timeComponentTemplate, this.transform);
        timeComponents.Add(timeComponent);
        return timeComponent;
    }

    public void setGCSLocation(GCS loc) {
        GCSlocation = loc;
    }

    public void setInScene(GCS relativeOrigin) {
        // Set the position of the object relative to the user
        double latDiff = (relativeOrigin.Latitude - GCSlocation.Latitude);
        double lonDiff = (relativeOrigin.Longitude - GCSlocation.Longitude);
        double altDiff = (relativeOrigin.Altitude - GCSlocation.Altitude);
        sceneX = (float) (latDiff * GCSscaler);
        sceneY = (float) altDiff;
        sceneZ = (float) (lonDiff * GCSscaler);
        this.transform.position = new Vector3(sceneX,sceneY,sceneZ);
        // Only render the object if it is within a certain distance
        inRange = Math.Pow(latDiff,2) + Math.Pow(lonDiff,2) < sceneRange;
        /*Debug.Log(" userlat: " + relativeOrigin.Latitude.ToString() + 
            " userlong: " + relativeOrigin.Longitude.ToString() +
            " useralt: " + relativeOrigin.Altitude.ToString() +
            " objlat: " + GCSlocation.Latitude.ToString() + 
            " objlong: " + GCSlocation.Longitude.ToString() +
            " objalt: " + GCSlocation.Altitude.ToString() + 
            " scenex " + sceneX.ToString() + 
            " sceney " + sceneY.ToString() + 
            " scenez " + sceneZ.ToString());*/

        gameObject.SetActive(inRange);
    }
}
