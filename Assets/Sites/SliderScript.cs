using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slide;
    public SiteHolderScript sitesScript;
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
        // If neccersary adjusting the available date range.
        if (sitesScript.AvailableDates.Count > 0) {
            slide.minValue = sitesScript.AvailableDates.Min;
            slide.maxValue = sitesScript.AvailableDates.Max;
        } else {
            slide.minValue = 0;
            slide.maxValue = 0;
        }
        // Setting the current date pointer.
        if (sitesScript.SelectedDate < slide.maxValue) {
            if (sitesScript.SelectedDate > slide.minValue)
                setDate(sitesScript.SelectedDate);
            else 
                setDate((int)slide.minValue);
        }
        else 
            setDate((int)slide.maxValue);
    }

    void setDate(int date) {
        slide.value = date;
        sitesScript.SelectedDate = date;
    }
}
