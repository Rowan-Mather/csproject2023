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
    public string repoURL = 
        "https://raw.githubusercontent.com/Rowan-Mather/csproject2023/main/";
    private string objectListFile = "object-list.txt";
    private string metadataFile = "/metadata.txt";
    private string[] objectArray;
    void Start()
    {
        // Determine list of objects to load
        loadObjectArray();
        // Loads in each one in turn
        foreach (string name in objectArray) {
            loadObject(name);
        }
    }

    // Reads the string array of names of objects from the webserver which are 
    // available to be imported and puts it in objectArray.
    void loadObjectArray() {
        var www = new WWW(repoURL + objectListFile);
        while (!www.isDone) System.Threading.Thread.Sleep(1);
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        string rawText = Encoding.ASCII.GetString(textStream.ToArray());
        objectArray = rawText.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
    }

    // Reads a model, (it's texture), and location from the webserver and
    // places it into the scene. Takes the name of the folder with all this data
    public GameObject loadObject(string name) {
        // Get the location
        Debug.Log(repoURL + name + metadataFile);
        var metadataURL = new WWW(repoURL + name + metadataFile);
        while (!metadataURL.isDone) System.Threading.Thread.Sleep(1);
        var metadataStream = new MemoryStream(Encoding.UTF8.GetBytes(metadataURL.text));
        string metadata = Encoding.ASCII.GetString(metadataStream.ToArray());

        // Load the object
        var objectURL = new WWW(repoURL + name + "/" + name + ".obj");
        while (!objectURL.isDone) System.Threading.Thread.Sleep(1);
        var objStream = new MemoryStream(Encoding.UTF8.GetBytes(objectURL.text));
        var loadedObj = new OBJLoader().Load(objStream);
        
        // ooh you can also specify an mtl path!!! yay in Load its overwridden
        // Load the texture (todo)
        // You can specify an mtl path also using the OBJLoader().Load(obj,mtl);

        // Places the object in the read location
        float x = float.Parse(metadata.Substring(0,5), 
            CultureInfo.InvariantCulture.NumberFormat);
        float y = float.Parse(metadata.Substring(6,5), 
            CultureInfo.InvariantCulture.NumberFormat);
        float z = float.Parse(metadata.Substring(12,5),
            CultureInfo.InvariantCulture.NumberFormat);
        loadedObj.transform.Translate(x, y, z);

        return loadedObj;
        
    }

}