using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SiteHolderScript : MonoBehaviour
{
    public UserLocationScript userLocation;
    public GameObject siteTemplate;
    public List<GameObject> sites = new List<GameObject>();
    private SortedSet<int> availableDates = new SortedSet<int>();
    public SortedSet<int> AvailableDates
    {
        get { return availableDates; }
    }
    private int selectedDate = 0;
    public int SelectedDate
    {
        get { return selectedDate; }
        set { selectedDate = value; }
    }

    public TMP_Text siteListText;

    // Update is called once per frame
    void Update()
    {
        availableDates.Clear();
        // Places each site in the world dependent on the users location.
        GCS uL = userLocation.getLocation();
        foreach (GameObject s in sites) {
            var siteScript = s.GetComponent<HistoricalSiteScript>();
            siteScript.setInScene(uL, selectedDate);
            if (siteScript.isRendered()) {
                availableDates.UnionWith(siteScript.SpecifiedTimes);
            }
        }
    }
    
    public GameObject addEmptySite(string title) {
        // Create a new site parent for the model, set its name.
        GameObject site = Instantiate(siteTemplate, this.transform);
        site.name = "Site: " + title;
        siteListText.text += title + "\n";
        /*
        // Add the model as a child
        siteModel.transform.SetParent(site.transform);
        // Set location
        var siteScript = site.GetComponent<SiteScript>();
        siteScript.setGCSLocation(location);
        siteScript.setInScene(userLocation.getLocation());*/
        // Put the parent in the site list
        sites.Add(site);
        return site;
    }

    public void removeSite(GameObject site) {
        sites.Remove(site);
        Destroy(site);
        siteListText.text = "";
        foreach (GameObject s in sites) {
            siteListText.text += site.name.Substring(6) + "\n";
        }
    }
}
