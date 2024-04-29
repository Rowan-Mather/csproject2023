using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using System.Globalization;
using UnityEngine.UI;

/*
Responsible for importing all objects and text from the server and placing them 
in the site hierarchy of 
    SiteHolder -> HistoricalSite -> TimeComponent -> Tag 
                                                 |-> Wavefront object (model)
*/
public class ImporterScript : MonoBehaviour
{
    // The central online repository for all sites
    private string repoURL = 
        "https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/";
    // The name of the master list of sites to import from the repo
    private string objectListFile = "object-list.txt";
    // Each site has a metadata file which contains all the non-model 
    // information about it 
    private string metadataFile = "metadata.txt";
    // Metadata should be of the form:
    /*
    location:1.04,284,343;
    model_name:church0|
        date:1560|
        tag:myinformation,2.35,5.43,2.5|
        tag:anotherthing,2.4,343,34;
    model_name:castle4|date:1069|tag:ruined,2.35,3.43,2.5     
    */

    // Link to the site holder script
    public SiteHolderScript siteHolderScript;
    // The prefab (template) which all site objects are created from
    public GameObject siteTemplate;
    // The code version of the objectListFile - the names of all sites being
    // imported into the app
    private string[] siteArray;


    void Start()
    {
        // Determine list of objects to load
        loadSiteArray();
        // Loads in each one in turn
        foreach (string name in siteArray) {
            if (name != "") {
                loadSite(name);
            }
        }
    }

    // Reads the string array of names of objects from the webserver which are 
    // available to be imported and puts it in siteArray.
    void loadSiteArray() {
        var www = new WWW(repoURL + objectListFile);
        while (!www.isDone) System.Threading.Thread.Sleep(1);
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        string rawText = Encoding.ASCII.GetString(textStream.ToArray());
        siteArray = rawText.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
    }

    // The main function for importing a site. Gets all models for it with their 
    // respective textures, dates and tags where listed, and the location. 
    // Each site can have multiple time components each corresponding to a 3D 
    // model representing a different point in time.
    private void loadSite(string siteName) {
        Debug.Log("Downloading site: " + siteName);
        // Get the raw string data for the metadata file
        string locURL = repoURL + siteName + "/" + metadataFile;
        var metadataURL = new WWW(locURL);
        while (!metadataURL.isDone) System.Threading.Thread.Sleep(1);
        var metadataStream = new MemoryStream(Encoding.UTF8.GetBytes(metadataURL.text));
        if (metadataStream == null) { Debug.Log("Failed to load site: " + siteName); return; }
        string metadata = Encoding.ASCII.GetString(metadataStream.ToArray());
        // Remove all newlines from metadata
        metadata = removeLineEndings(metadata);

        // Instantiates an empty historical site object.
        GameObject site = siteHolderScript.addEmptySite(siteName);
        HistoricalSiteScript siteScript = site.GetComponent<HistoricalSiteScript>();

        // Split metadata string into its tags.
        string[] metadataSplit = metadata.Split(";");
        foreach (string modelMeta in metadataSplit) {
            if (modelMeta.StartsWith("location:")) {
                loadLocation(siteScript, modelMeta);
            }
            else {
                // Create a new time component and add it to the site.
                GameObject tc = siteScript.addEmptyTimeComponent();
                SiteTimeComponentScript tcScript 
                    = tc.GetComponent<SiteTimeComponentScript>();
                foreach (string datum in modelMeta.Split("|")) {
                    var subDatum = datum.Split(":");
                    switch (subDatum[0]) {
                        case "model_name":
                            tc.name = "Time: " + subDatum[1];
                            var model = loadObject(siteName, subDatum[1]);
                            model.transform.SetParent(tc.transform);
                            break;
                        case "date":
                            loadDate(tcScript, subDatum[1]);
                            break;
                        case "tag":
                            loadTag(tcScript, subDatum[1]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        siteScript.updateSpecifiedTimes();
    }

    // Decodes the location part of the metadata file to extract the latitude,
    // longitude and altitude
    private void loadLocation(HistoricalSiteScript site, string locationData) {
        var locationValues = locationData.Substring(9).Split(",");
        Debug.Log(locationValues[0] + " " + locationValues[1] + " " + locationValues[2]);
        try {
            double lat = Double.Parse(locationValues[0]);
            double lon = Double.Parse(locationValues[1]);
            double alt = Double.Parse(locationValues[2]);
            site.setGCSLocation(new GCS(lon, lat, alt));
        }
        catch {
            Debug.Log("Failed to load site location.");
        }
    }

    // Decodes the date part of the metadata file. Dates can be listed as
    // AD/BC/CE/BCE. CE dates represented as +ve integers and BCE as -ve.
    private void loadDate(SiteTimeComponentScript tc, string dateData) {
        try {
            if (dateData.Length >= 3) {
                string prefix = dateData.Substring(0,dateData.Length-2);
                string suffix = dateData.Substring(dateData.Length-2);
                switch (suffix) {
                    case "BC":
                        tc.Date = -1*Int32.Parse(prefix);
                        return;
                    case "AD":
                        tc.Date = Int32.Parse(prefix);
                        return;
                    case "CE":
                        if (dateData[dateData.Length-3] == 'B')
                            tc.Date = -1*Int32.Parse(dateData.Substring(0,dateData.Length-3));
                        else
                            tc.Date = Int32.Parse(prefix);
                        return;
                }
            }
            tc.Date = Int32.Parse(dateData);
        }
        catch {
            tc.Date = null;
        }
    }

    // Decodes the tag part of the metadata file. Time components can have 
    // labels which are placed in locations relative to the model.
    private void loadTag(SiteTimeComponentScript tc, string tagData) {
        string[] tagSplit = tagData.Split(",");
        try {
            float x = (float) Double.Parse(tagSplit[1]);
            float y = (float) Double.Parse(tagSplit[2]);
            float z = (float) Double.Parse(tagSplit[3]);
            tc.addTag(tagSplit[0], x, y, z);
        }
        catch {}
    }

    // Reads a model, (it's texture), and location from the webserver and
    // places it into the scene. Takes the name of the folder with all this data
    public GameObject loadObject(string siteName, string modelName) {
        // Get the texture (.mtl)
        string mtlurl = repoURL + siteName + "/" + modelName + ".mtl";
        var mtlURL = new WWW(mtlurl);
        while (!mtlURL.isDone) System.Threading.Thread.Sleep(1);
        var mtlStream = new MemoryStream(Encoding.UTF8.GetBytes(mtlURL.text));

        // Load the object
        string objurl = repoURL + siteName + "/" + modelName + ".obj";
        var objectURL = new WWW(objurl);
        while (!objectURL.isDone) System.Threading.Thread.Sleep(1);
        var objStream = new MemoryStream(Encoding.UTF8.GetBytes(objectURL.text));
        GameObject loadedObj;
        if (mtlStream == null) {
            loadedObj = new OBJLoader().Load(objStream);
            Debug.Log("Cannot load texture for: " + modelName);
        }
        else {
            loadedObj = new OBJLoader().Load(objStream, mtlStream);
            Debug.Log("Loaded texture for: " + modelName);
        }
        // ooh you can also specify an mtl path!!! yay in Load its overwridden
        // Load the texture (todo)
        // You can specify an mtl path also using the OBJLoader().Load(obj,mtl);

        if (loadedObj == null) {
            Debug.Log("Failed to load model: " + modelName);
            return null;
        }
        return loadedObj;
    }

    // Helper function to remove characters from the metadata file for processing
    // https://stackoverflow.com/questions/6750116/how-to-eliminate-all-line-breaks-in-string
    private static string removeLineEndings(string value)
    {
        if(String.IsNullOrEmpty(value))
        {
            return value;
        }
        string lineSeparator = ((char) 0x2028).ToString();
        string paragraphSeparator = ((char)0x2029).ToString();

        return value.Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace(lineSeparator, string.Empty)
                    .Replace(paragraphSeparator, string.Empty);
    }
}