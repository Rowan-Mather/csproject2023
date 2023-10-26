using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteScript : MonoBehaviour
{
    public double GCSscaler = 1;
    private GCS GCSlocation = new GCS();
    private float sceneX = 0;
    private float sceneY = 0;
    private float sceneZ = 0;

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
        //loadedObj.transform.Translate((float)location.Latitude, (float)location.Longitude, 1);
    }

    public void setSceneLocation(GCS relativeOrigin) {
        sceneX = (float)
            ((relativeOrigin.Latitude - GCSlocation.Latitude) * GCSscaler);
        sceneY = 0;
        sceneZ = (float)
            ((relativeOrigin.Longitude - GCSlocation.Longitude) * GCSscaler);
        this.transform.Translate(sceneX, sceneY, sceneZ);
    }
}
