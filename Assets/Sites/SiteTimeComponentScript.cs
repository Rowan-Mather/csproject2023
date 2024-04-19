using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The class corresponding to a model in a site with optional date and tagging 
information. Can create tags and child them to this object.
*/
public class SiteTimeComponentScript : MonoBehaviour
{
    // Date of time component
    private int? date;
    public int? Date
    {
        get { return date; }
        set { date = value; }
    }
    // Prefab for creating a tag
    public GameObject tagTemplate;
    // List of all tags childed to this object
    public List<GameObject> tags = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {}

    // Shows or hides the time component based on the currently selected date
    // on the timeline slider
    public void showInTime(int? currentSelectedDate) {
        if (date == null || currentSelectedDate == null) {
            gameObject.SetActive(true);
            return;
        }
        gameObject.SetActive(currentSelectedDate == date);
    }

    // Creates a tag given its text and relative position
    public void addTag(string text, float x, float y, float z) {
        GameObject tag = Instantiate(tagTemplate, this.transform);
        tag.transform.SetParent(this.transform);
        var tagScript = tag.GetComponent<TagScript>();
        tagScript.setRelativePosition(x,y,z);
        tagScript.setText(text);
        tags.Add(tag);
    }
}
