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

public class ImporterScript : MonoBehaviour
{
    public GameObject coventryCathedral;
    public GameObject computer;
    public GameObject train;
    public GameObject temple;
    public GameObject stone;
    public Text testFeedback;

    public GameObject siteTemplate;
    public SiteHolderScript siteHolderScript;
    private string repoURL = 
        "https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/";
    private string objectListFile = "object-list.txt";
    private string metadataFile = "metadata.txt";
    private string[] siteArray;


    void Start()
    {
        /* HARD CODED TEST SITE *//////////
        /*
        GameObject site1 = siteHolderScript.addEmptySite("Demo");
        HistoricalSiteScript siteScript1 = site1.GetComponent<HistoricalSiteScript>();
        //siteScript1.setGCSLocation(new GCS(-1.548796, 52.37359, 0));
        siteScript1.setGCSLocation(new GCS(-1.552912, 52.37531, 0));
        //52.3737 -1.548767 139.9 48
        //52.37359 -1.548796 139.4
        //52.38356 -1.560355 131.5 20
        // 52.38356 -1.56035 131.5
        //52.38371 -1.560281 135.5 39.6

        //Computer
        GameObject tc1_1 = siteScript1.addEmptyTimeComponent();
        SiteTimeComponentScript tc1_1Script 
            = tc1_1.GetComponent<SiteTimeComponentScript>();
        tc1_1.name = "Time: Modern Build";
        computer.transform.SetParent(tc1_1.transform);
        tc1_1Script.Date = 1980;

        //Train
        GameObject tc1_2 = siteScript1.addEmptyTimeComponent();
        SiteTimeComponentScript tc1_2Script 
            = tc1_2.GetComponent<SiteTimeComponentScript>();
        tc1_2.name = "Time: Industrial Steam Train";
        train.transform.SetParent(tc1_2.transform);
        tc1_2Script.Date = 1830;

        //Temple
        GameObject tc1_3 = siteScript1.addEmptyTimeComponent();
        SiteTimeComponentScript tc1_3Script 
            = tc1_3.GetComponent<SiteTimeComponentScript>();
        tc1_3.name = "Time: Mayan Temple";
        temple.transform.SetParent(tc1_3.transform);
        tc1_3Script.Date = 750;

        //Stone
        GameObject tc1_4 = siteScript1.addEmptyTimeComponent();
        SiteTimeComponentScript tc1_4Script 
            = tc1_4.GetComponent<SiteTimeComponentScript>();
        tc1_4.name = "Time: Monolith";
        stone.transform.SetParent(tc1_4.transform);
        tc1_4Script.Date = -3000;*/

        GameObject site1 = siteHolderScript.addEmptySite("Coventry Cathedral");
        HistoricalSiteScript siteScript1 = site1.GetComponent<HistoricalSiteScript>();
        //siteScript1.setGCSLocation(new GCS(-1.548796, 52.37359, 0));
        siteScript1.setGCSLocation(new GCS(-1.548739, 52.40795814, 0));
        // home  52.37374 -1.548739 138
        // cov 52.40795814693444, -1.5074476429870276

        GameObject tc1_4 = siteScript1.addEmptyTimeComponent();
        SiteTimeComponentScript tc1_4Script 
            = tc1_4.GetComponent<SiteTimeComponentScript>();
        tc1_4.name = "Time: Before bombing";
        coventryCathedral.transform.SetParent(tc1_4.transform);
        tc1_4Script.Date = 1500;

        tc1_4Script.addTag("Columns", -13.0f, 1.4f, 0f);
        tc1_4Script.addTag("Spire", -7.0f, 1.0f, -10.0f);

        siteScript1.updateSpecifiedTimes();

        //testFeedback.text += "1.";

        // Determine list of objects to load
        loadSiteArray();
        // Loads in each one in turn
        foreach (string name in siteArray) {
            if (name != "") {
                loadSite(name);
            }
        }

        testFeedback.text += "2.";

        /*
        // Test cube
        var objectURL = new WWW("https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/dated/cone.obj");
        while (!objectURL.isDone) System.Threading.Thread.Sleep(1);
        var objStream = new MemoryStream(Encoding.UTF8.GetBytes(objectURL.text));
        GameObject loadedObj = new OBJLoader().Load(objStream);

        string mtlurl = "https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/test-house/test-house.mtl";
        var mtlURL = new WWW(mtlurl);
        while (!mtlURL.isDone) System.Threading.Thread.Sleep(1);
        var mtlStream = new MemoryStream(Encoding.UTF8.GetBytes(mtlURL.text));

        var objectURL2 = new WWW("https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/test-house/test-house.obj");
        while (!objectURL2.isDone) System.Threading.Thread.Sleep(1);
        var objStream2 = new MemoryStream(Encoding.UTF8.GetBytes(objectURL2.text));
        GameObject loadedObj2 = new OBJLoader().Load(objStream2, mtlStream);
        loadedObj2.transform.position += new Vector3(4,0,0);

        testFeedback.text = "testing import";
        if (objStream == null) testFeedback.text += "; stream null";
        else { testFeedback.text += "; stream imported"; }
        if (loadedObj == null) testFeedback.text += "; object null";
        else { testFeedback.text += "; object imported"; }*/
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

    private void loadLocation(HistoricalSiteScript site, string locationData) {
        var locationValues = locationData.Substring(9).Split(",");
        Debug.Log(locationValues[0] + " " + locationValues[1] + " " + locationValues[2]);
        try {
            double lat = Double.Parse(locationValues[0]);
            double lon = Double.Parse(locationValues[1]);
            double alt = 0; //Double.Parse(locationValues[2]);
            site.setGCSLocation(new GCS(lon, lat, alt));
        }
        catch {
            Debug.Log("Failed to load site location.");
        }
    }

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