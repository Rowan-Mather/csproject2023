using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using System.Globalization;

public class ImporterScript : MonoBehaviour
{
    public GameObject siteTemplate;
    public SiteHolderScript siteHolderScript;
    private string repoURL = 
        "https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/";
    private string objectListFile = "object-list.txt";
    private string metadataFile = "metadata.txt";
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
        //var www = new WWW("https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/object-list.txt");
        while (!www.isDone) System.Threading.Thread.Sleep(1);
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        string rawText = Encoding.ASCII.GetString(textStream.ToArray());
        siteArray = rawText.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
    }

    // Metadata should be of the form:
    /*
    location:1.04,284,343;

    model_name:church0|
    date:1560|
    tag:myinformation,2.35,5.43,2.5|
    tag:anotherthing,2.4,343,34;

    model_name:castle4|date:1069|tag:ruined,2.35,3.43,2.5     
    */

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
                foreach (string datum in modelMeta.Split("|")) {
                    // Create a new time component and add it to the site.
                    GameObject tc = siteScript.addEmptyTimeComponent();
                    SiteTimeComponentScript tcScript 
                        = tc.GetComponent<SiteTimeComponentScript>();

                    var subDatum = datum.Split(":");
                    switch (subDatum[0]) {
                        case "model_name":
                            tc.name = subDatum[1];
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
    }

    private void loadLocation(HistoricalSiteScript site, string locationData) {
        var locationValues = locationData.Split(",");
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

    private void loadDate(SiteTimeComponentScript tc, string dateData) {
        tc.Date = dateData;
    }

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