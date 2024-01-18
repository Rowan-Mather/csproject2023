using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteScript : MonoBehaviour
{
    public const double GCSscaler = 111139;
    private double sceneRange = Math.Pow(0.01,2);
    private GCS GCSlocation = new GCS();
    private float sceneX = 0;
    private float sceneY = 0;
    private float sceneZ = 0;
    bool inRange = false;

    // Start is called before the first frame update
    void Start() {}

    public void setGCSLocation(GCS loc) {
        GCSlocation = loc;
    }

    public void setInScene(GCS relativeOrigin) {
        // Set the position of the object relative to the user
        double latDiff = (relativeOrigin.Latitude - GCSlocation.Latitude);
        double lonDiff = (relativeOrigin.Longitude - GCSlocation.Longitude);
        double altDiff = (relativeOrigin.Altitude - GCSlocation.Altitude);
        sceneX = (float) (latDiff * GCSscaler);
        sceneY = (float) (altDiff * GCSscaler);
        sceneZ = (float) (lonDiff * GCSscaler);
        this.transform.position = new Vector3(sceneX,sceneY,sceneZ);
        // Only render the object if it is within a certain distance
        inRange = Math.Pow(latDiff,2) + Math.Pow(lonDiff,2) < sceneRange;
        Debug.Log(" userlat: " + relativeOrigin.Latitude.ToString() + 
            " userlong: " + relativeOrigin.Longitude.ToString() +
            " useralt: " + relativeOrigin.Altitude.ToString() +
            " objlat: " + GCSlocation.Latitude.ToString() + 
            " objlong: " + GCSlocation.Longitude.ToString() +
            " objalt: " + GCSlocation.Altitude.ToString());

        gameObject.SetActive(inRange); 
    }
}
