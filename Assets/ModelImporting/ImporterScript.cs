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
    public SiteHolderScript siteHolderScript;
    private string repoURL = 
        "https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/";
    private string objectListFile = "object-list.txt";
    private string metadataFile = "metadata.txt";
    private string[] objectArray;
    void Start()
    {
        // Determine list of objects to load
        loadObjectArray();
        // Loads in each one in turn
        foreach (string name in objectArray) {
            if (name != "") loadObject(name);
        }
    }

    // Reads the string array of names of objects from the webserver which are 
    // available to be imported and puts it in objectArray.
    void loadObjectArray() {
        var www = new WWW(repoURL + objectListFile);
        //var www = new WWW("https://raw.githubusercontent.com/Rowan-Mather/csproject2023/sites/object-list.txt");
        while (!www.isDone) System.Threading.Thread.Sleep(1);
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        string rawText = Encoding.ASCII.GetString(textStream.ToArray());
        objectArray = rawText.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
    }
    //todo: read alt too
    public GCS readLocation(string siteName) {
        // Get the raw string data
        string locURL = repoURL + siteName + "/" + metadataFile;
        //Debug.Log(locURL);
        var metadataURL = new WWW(locURL);
        while (!metadataURL.isDone) System.Threading.Thread.Sleep(1);
        var metadataStream = new MemoryStream(Encoding.UTF8.GetBytes(metadataURL.text));
        string metadata = Encoding.ASCII.GetString(metadataStream.ToArray());
        
        string[] metavalues = metadata.Split(",");
        double lon = Double.Parse(metavalues[0]);
        double lat = Double.Parse(metavalues[1]);
        double alt = Double.Parse(metavalues[2]);
        
        lat = 52.379378009744535;
        lon = -1.5604757047899998;
        return new GCS(lon, lat, alt);
        // Places the object in the read location
        /*float x = float.Parse(metadata.Substring(0,5), 
            CultureInfo.InvariantCulture.NumberFormat);
        float y = float.Parse(metadata.Substring(6,5), 
            CultureInfo.InvariantCulture.NumberFormat);
        float z = float.Parse(metadata.Substring(12,5),
            CultureInfo.InvariantCulture.NumberFormat);*/
    }

    // Reads a model, (it's texture), and location from the webserver and
    // places it into the scene. Takes the name of the folder with all this data
    public GameObject loadObject(string siteName) {
        Debug.Log("Loading site: " + siteName);
        GCS loc = readLocation(siteName);

        // Get the texture (.mtl)
        string mtlurl = repoURL + siteName + "/" + siteName + ".mtl";
        var mtlURL = new WWW(mtlurl);
        while (!mtlURL.isDone) System.Threading.Thread.Sleep(1);
        var mtlStream = new MemoryStream(Encoding.UTF8.GetBytes(mtlURL.text));

        // Load the object
        string objurl = repoURL + siteName + "/" + siteName + ".obj";
        var objectURL = new WWW(objurl);
        while (!objectURL.isDone) System.Threading.Thread.Sleep(1);
        var objStream = new MemoryStream(Encoding.UTF8.GetBytes(objectURL.text));
        GameObject loadedObj;
        if (mtlStream == null) {
            loadedObj = new OBJLoader().Load(objStream);
            Debug.Log("cannot load texture for: " + siteName);
        }
        else {
            loadedObj = new OBJLoader().Load(objStream, mtlStream);
            Debug.Log("loaded texture for: " + siteName);
        }
        // ooh you can also specify an mtl path!!! yay in Load its overwridden
        // Load the texture (todo)
        // You can specify an mtl path also using the OBJLoader().Load(obj,mtl);

        //try {
            siteHolderScript.addSite(loadedObj, siteName, loc);
        //}
        //catch {
        //    Debug.Log("Failed to load site: " + siteName);
        //}
        //loadedObj.transform.Translate((float)loc.Latitude, (float)loc.Longitude, 1);
        return loadedObj;
    }
}