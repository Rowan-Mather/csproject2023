using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteHolderScript : MonoBehaviour
{
    public UserLocationScript userLocation;
    public GameObject siteTemplate;
    public List<GameObject> sites = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GCS uL = userLocation.getLocation();
        foreach (GameObject s in sites) {
            s.GetComponent<SiteScript>().setInScene(uL);
        }
    }
    
    public void addSite(GameObject siteModel, string title, GCS location) {
        // Create a new site parent for the model, set its name
        GameObject site = Instantiate(siteTemplate, this.transform);
        site.name = "Site: " + title;
        // Add the model as a child
        siteModel.transform.SetParent(site.transform);
        // Set location
        var siteScript = site.GetComponent<SiteScript>();
        siteScript.setGCSLocation(location);
        siteScript.setInScene(userLocation.getLocation());
        // Put the parent in the site list
        sites.Add(site);
    }
}
