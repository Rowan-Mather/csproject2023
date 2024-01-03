using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDisplayScript : MonoBehaviour {
    //public UserLocationScript userLocation;
    public Text textLoc;
    void Start() {}

    public void updateDisplay(GCS loc) {
        textLoc.text = loc.toString();
    }
}
