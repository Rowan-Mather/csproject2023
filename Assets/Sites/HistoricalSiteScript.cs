using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HistoricalSiteScript : MonoBehaviour
{
    /* Available Models (with tags) */
    // These can optionally have a particular time period allocated.
    public GameObject timeComponentTemplate;
    private List<GameObject> timeComponents = new List<GameObject>();
    private SortedSet<int> specifiedTimes = new SortedSet<int>();
    public SortedSet<int> SpecifiedTimes
    {
        get { return specifiedTimes; }
    }

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

    public bool isRendered() { return inRange; }

    // Add one of the time component prefab instances.
    public GameObject addEmptyTimeComponent() {
        GameObject timeComponent = Instantiate(timeComponentTemplate, this.transform);
        timeComponents.Add(timeComponent);
        return timeComponent;
    }

    public void updateSpecifiedTimes() {
        specifiedTimes.Clear();
        foreach (var comp in timeComponents) {
            var compScr = comp.GetComponent<SiteTimeComponentScript>();
            if (compScr.Date != null) {
                specifiedTimes.Add((int)compScr.Date);
            }
        }
    }

    public void setGCSLocation(GCS loc) {
        GCSlocation = loc;
    }

    public void setInScene(GCS relativeOrigin, int? date = null) {
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

        // Finds the most available time before/at the selected time.
        // Enables/disables time components accordingly.
        int? recent = timeSearch(date);
        if (recent == null) {}
        foreach (var comp in timeComponents) {
            var compScr = comp.GetComponent<SiteTimeComponentScript>();
            compScr.showInTime(recent);
        }
    }

    // Performs a binary search on the available time set, but if there is no
    // exact match, takes the date nearest but before the given date.
    private int? timeSearch(int? date) {
        if (date == null || specifiedTimes.Count == 0 || date < specifiedTimes.Min) 
            return null;

        int l = 0;
        int r = specifiedTimes.Count-1;
        int m = 0;
        while (l <= r) {
            m = (l+r)/2;
            var elem = specifiedTimes.ElementAt(m);
            if (elem < date) l = m+1;
            else if (elem > date) r = m-1;
            else return date;
        }
        if (specifiedTimes.ElementAt(m) < date) return specifiedTimes.ElementAt(m);
        else return specifiedTimes.ElementAt(m-1);        
    }
}
