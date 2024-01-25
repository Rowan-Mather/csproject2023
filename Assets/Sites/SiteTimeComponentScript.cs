using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteTimeComponentScript : MonoBehaviour
{
    private string date = ""; 
    public string Date
    {
        get { return date; }
        set { date = value; }
    }
    GameObject tagTemplate;
    public List<GameObject> tags = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {}

    public void addTag(string text, float x, float y, float z) {
        GameObject tag = Instantiate(tagTemplate, this.transform);
        tag.transform.SetParent(this.transform);
        var tagScript = tag.GetComponent<TagScript>();
        tagScript.setRelativePosition(x,y,z);
        tagScript.setText(text);
        tags.Add(tag);
    }
}
