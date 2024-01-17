using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteScript : MonoBehaviour
{
    public double GCSscaler = 1;
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
        double latDiff = (relativeOrigin.Latitude - GCSlocation.Latitude) * GCSscaler;
        double lonDiff = (relativeOrigin.Longitude - GCSlocation.Longitude) * GCSscaler;
        double altDiff = (relativeOrigin.Altitude - GCSlocation.Altitude) * GCSscaler;
        sceneX = (float) latDiff;
        sceneY = (float) altDiff;
        sceneZ = (float) lonDiff;
        this.transform.position = new Vector3(sceneX,sceneY,sceneZ);
        // Only render the object if it is within a certain distance
        inRange = Math.Pow(latDiff,2) + Math.Pow(lonDiff,2) < sceneRange;
        /*Debug.Log(" userlat: " + relativeOrigin.Latitude.ToString() + 
            " userlong: " + relativeOrigin.Longitude.ToString() +
            " useralt: " + relativeOrigin.Altitude.ToString() +
            " objlat: " + GCSlocation.Latitude.ToString() + 
            " objlong: " + GCSlocation.Longitude.ToString() +
            " objalt: " + GCSlocation.Altitude.ToString());*/

        gameObject.SetActive(inRange); 
    }
}
