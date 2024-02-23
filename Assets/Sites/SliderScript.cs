using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    public Slider slide;
    public SiteHolderScript sitesScript;
    public GameObject labelTemplate;
    private SortedSet<int> dateSet = new SortedSet<int>();
    private List<GameObject> labels = new List<GameObject>();
    public LocationDisplayScript locDisplay;

    // Start is called before the first frame update
    void Start()
    {
        slide.wholeNumbers = true;
        slide.minValue = 0;
        slide.maxValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Need to adjust date range and text labels.
        if (!dateSet.Equals(sitesScript.AvailableDates)) {
            if (sitesScript.AvailableDates.Count > 0) {
                slide.minValue = sitesScript.AvailableDates.Min;
                slide.maxValue = sitesScript.AvailableDates.Max;
            } else {
                slide.minValue = 0;
                slide.maxValue = 0;
            }
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

    void setDate(int date) {
        slide.value = date;
        sitesScript.SelectedDate = date;
    }

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
