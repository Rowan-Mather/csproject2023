using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDisplayScript : MonoBehaviour {
    //public UserLocationScript userLocation;
    private GCS location = new GCS();
    private string date = "0CE";
    public Text textLoc;
    void Start() {}

    public void updateLocationDisplay(GCS loc) {
        this.location = loc;
        updateDisplay();
    }

    public void updateDateDisplay(int d) {
        this.date = d < 0 ? 
            d.ToString().Substring(1) + "BCE" : 
            d.ToString() + "CE";
        updateDisplay();
    }

    private void updateDisplay() {
        textLoc.text = "GPS: " + location.toString() + 
            "\nYear: " + date;
    }
}
