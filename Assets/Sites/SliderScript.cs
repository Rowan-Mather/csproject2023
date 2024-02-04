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
        sitesScript.SelectedDate = (int)slide.value;            
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
            float x = (date - slide.minValue) / (slide.maxValue - slide.minValue) * 500 - 250;
            timeLabel.transform.position += new Vector3(x,-200f,0f);
            timeLabel.GetComponent<TMP_Text>().text = date.ToString();
            labels.Add(timeLabel);
        }
    }
}
