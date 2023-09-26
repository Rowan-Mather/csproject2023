using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine.Networking;

public class ImporterScript : MonoBehaviour
{
    private string repoURL = "https://raw.githubusercontent.com/Rowan-Mather/project-testing2023/main/";
    private string objectListFile = "object-list.txt";
    private string metadataFile = "/metadata.txt";
    private string[] objectArray;
    void Start()
    {
        // Determine list of objects to load
        loadObjectArray();
        foreach (string name in objectArray) {
            loadObject(name);
        }
       
        
    }

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
        loadedObj.transform.Translate(UnityEngine.Random.Range(-3, 3), 1, 1);
        return loadedObj;
        // ooh you can also specify an mtl path!!! yay in Load its overwridden
        // Put the object in the location
        
    }

}

        /*
        UnityWebRequest www = UnityWebRequest.Get(repoURL + objectListFile);
        www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            string raw = www.downloadHandler.text;
            // Show results as text
            Debug.Log("hehe");
            Debug.Log(raw);
        }*/
