using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
The tags are the extra text information allocated to a time componenet 
(associated with a single model). Their position is stored relative to the model.
*/
// Used: https://gamedev.stackexchange.com/questions/116177/how-to-dynamically-create-an-ui-text-object-in-unity-5
public class TagScript : MonoBehaviour
{
    // The reference to the actual text component
    public TMP_Text text;

    // Sets the position of the tag in the editor.
    public void setRelativePosition(float x, float y, float z) {
        gameObject.transform.position += new Vector3(x,y,z);
        
    }

    // Sets the tag text
    public void setText(string t) {
        //GetComponent<TextMesh>().text = t;
        text.text = t;
    }
 
    void Start() {
        text = this.gameObject.GetComponent<TextMeshPro>();
    }

    // Rotates the text round to face the virtual camera so it is always legible
    void Update() {
        transform.localEulerAngles = Quaternion.LookRotation(
            Camera.main.transform.position - transform.position, Vector3.up).eulerAngles;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
    }
}
