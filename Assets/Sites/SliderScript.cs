using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
The controller for the timeline slider. Stores currently available dates, 
displays them as labels and updates the selected date in the SiteHolder.
*/
public class SliderScript : MonoBehaviour
{
    // Link to SiteHolderScript
    public SiteHolderScript sitesScript;
    // Link to the location display class, which also displays the currently 
    // selected date
    public LocationDisplayScript locDisplay;
    // The reference for the slider object in the editor
    public Slider slide;
    // Set of all currently available dates from the sites in range
    private SortedSet<int> dateSet = new SortedSet<int>();
    // Prefab for creating a label for a date
    public GameObject labelTemplate;
    // The list of active label objects
    private List<GameObject> labels = new List<GameObject>();

    void Start()
    {
        // Initialise the slider
        slide.wholeNumbers = true;
        slide.minValue = 0;
        slide.maxValue = 0;
    }

    void Update()
    {
        // Checks if the slider labels correspond to the main store of dates
        // that are currently available
        if (!dateSet.Equals(sitesScript.AvailableDates)) {
            // If not, update them
            // Counts the number of dates so they can be distributed 
            // along the timeline scaling to the range
            if (sitesScript.AvailableDates.Count > 0) {
                // Sets the minimum and maximum values of the slider to the 
                // oldest and newest date 
                slide.minValue = sitesScript.AvailableDates.Min;
                slide.maxValue = sitesScript.AvailableDates.Max+1;
            } else {
                slide.minValue = 0;
                slide.maxValue = 0;
            }
            // Caps the slider
            if (sitesScript.SelectedDate > slide.maxValue) 
                setDate((int)slide.maxValue);
            if (sitesScript.SelectedDate < slide.minValue) 
                setDate((int)slide.minValue);
            dateSet = new SortedSet<int>(sitesScript.AvailableDates);
            generateLabels();
        }
        // Update slider position store.
        int date = (int)slide.value;
        sitesScript.SelectedDate = date;   
        locDisplay.updateDateDisplay(date); 
    }

    // Updates the site holder to the selected date
    void setDate(int date) {
        slide.value = date;
        sitesScript.SelectedDate = date;
    }

    // Creates all the labels for the current date set
    void generateLabels() {
        // Destroy existing labels.
        foreach (var label in labels) {
            DestroyImmediate(label);
        }
        labels.Clear();
        // Create new labels and position them according to the date itself.
        foreach (var date in dateSet) {
            GameObject timeLabel = Instantiate(labelTemplate, this.transform);
            timeLabel.transform.SetParent(this.transform);
            float x = ((date - slide.minValue) / (slide.maxValue - slide.minValue) * 600 - 300);
            timeLabel.transform.position += new Vector3(x,180f,0f);
            if (date < 0)
                timeLabel.GetComponent<TMP_Text>().text = date.ToString().Substring(1) + "BCE";
            else 
                timeLabel.GetComponent<TMP_Text>().text = date.ToString() + "CE";
            labels.Add(timeLabel);
        }
    }
}
