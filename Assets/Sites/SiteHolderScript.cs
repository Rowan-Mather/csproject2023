using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
The container and creator of all site objects. Hierarchy:
    SiteHolder -> HistoricalSite -> TimeComponent -> Tag 
                                                 |-> Wavefront object (model)
*/
public class SiteHolderScript : MonoBehaviour
{
    // Link to the location script
    public UserLocationScript userLocation;
    // Prefab for creating a site from 
    public GameObject siteTemplate;
    // List of all site objects childed to this class
    public List<GameObject> sites = new List<GameObject>();
    // The display text for the sites list 
    public TMP_Text siteListText;
    // The master list of all dates from the sites
    private SortedSet<int> availableDates = new SortedSet<int>();
    public SortedSet<int> AvailableDates
    {
        get { return availableDates; }
    }
    // The currently selected date from the timeline slider
    private int selectedDate = 0;
    public int SelectedDate
    {
        get { return selectedDate; }
        set { selectedDate = value; }
    }

    void Update()
    {
        // Places each site in the world dependent on the user's location and
        // if it is within render distance of the user, gets the time component
        // dates from it
        availableDates.Clear();
        GCS uL = userLocation.getLocation();
        foreach (GameObject s in sites) {
            var siteScript = s.GetComponent<HistoricalSiteScript>();
            siteScript.setInScene(uL, selectedDate);
            if (siteScript.isRendered()) {
                availableDates.UnionWith(siteScript.SpecifiedTimes);
            }
        }
    }
    
    // Creates a new site given its name and childs it to this class
    public GameObject addEmptySite(string title) {
        // Create a new site parent for the model, set its name.
        GameObject site = Instantiate(siteTemplate, this.transform);
        site.name = "Site: " + title;
        siteListText.text += title + "\n";
        // Put the parent in the site list
        sites.Add(site);
        return site;
    }

    // Deletes a site (unused)
    public void removeSite(GameObject site) {
        sites.Remove(site);
        Destroy(site);
        siteListText.text = "";
        foreach (GameObject s in sites) {
            siteListText.text += site.name.Substring(6) + "\n";
        }
    }
}
