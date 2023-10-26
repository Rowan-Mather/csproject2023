using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteScript : MonoBehaviour
{
    public double GCSscaler = 1;
    public double sceneRange = 5;
    private GCS GCSlocation = new GCS();
    private float sceneX = 0;
    private float sceneY = 0;
    private float sceneZ = 0;
    bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGCSLocation(GCS loc) {
        GCSlocation = loc;
    }

    public void setInScene(GCS relativeOrigin) {
        double latDiff = (relativeOrigin.Latitude - GCSlocation.Latitude) * GCSscaler;
        double lonDiff = (relativeOrigin.Longitude - GCSlocation.Longitude) * GCSscaler;
        sceneX = (float) latDiff;
        sceneY = 0;
        sceneZ = (float) lonDiff;
        //this.transform.Translate(sceneX, sceneY, sceneZ);
        this.transform.position = new Vector3(sceneX,sceneY,sceneZ);
        inRange = Math.Sqrt(Math.Pow(latDiff,2) + Math.Pow(lonDiff,2)) < sceneRange;
        gameObject.SetActive(inRange); 
    }
}
