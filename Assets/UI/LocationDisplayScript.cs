using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
The controller for the text display on the side bar showing the GPS location of 
the user and the current date selected.
*/
public class LocationDisplayScript : MonoBehaviour {
    // The reference to the actual text component
    public Text textLoc;
    // The GCS location and its string representation
    private GCS location = new GCS();
    private string date = "0CE";
    void Start() {}

    public void updateLocationDisplay(GCS loc) {
        this.location = loc;
        updateDisplay();
    }

    // Formats the given date to be in standard CE/BCE form and displays it.
    public void updateDateDisplay(int d) {
        this.date = d < 0 ? 
            d.ToString().Substring(1) + "BCE" : 
            d.ToString() + "CE";
        updateDisplay();
    }

    // Formats the stored location to be in standard degree, minute, second form
    // and displays it with the year
    private void updateDisplay() {
        textLoc.text = "GPS: " + location.toString() + 
            "\nYear: " + date;
    }
}
