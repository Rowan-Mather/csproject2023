using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TagScript : MonoBehaviour
{
    // private float siteX;
    // private float siteY;
    // private float siteZ;
    public TMP_Text text;

    public void setRelativePosition(float x, float y, float z) {
        gameObject.transform.position += new Vector3(x,y,z);
        
    }

    public void setText(string t) {
        //GetComponent<TextMesh>().text = t;
        text.text = t;
    }
    
//     GameObject CreateText(Transform canvas_transform, float x, float y, string text_to_print, int font_size, Color text_color)
// {
//     GameObject UItextGO = new GameObject("Text2");
//     UItextGO.transform.SetParent(canvas_transform);

//     RectTransform trans = UItextGO.AddComponent<RectTransform>();
//     trans.anchoredPosition = new Vector2(x, y);

//     Text text = UItextGO.AddComponent<Text>();
//     text.text = text_to_print;
//     text.fontSize = font_size;
//     text.color = text_color;

//     return UItextGO;
// }
    //https://gamedev.stackexchange.com/questions/116177/how-to-dynamically-create-an-ui-text-object-in-unity-5
    void Start() {
        text = this.gameObject.GetComponent<TextMeshPro>();
    }

    void Update() {
        transform.localEulerAngles = Quaternion.LookRotation(
            Camera.main.transform.position - transform.position, Vector3.up).eulerAngles;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        //Vector3 point = Quaternion.LookRotation(Camera.main.transform.eulerAngles).eulerAngles;
        //Debug.Log(point);
        //gameObject.transform.LookAt(Camera.main.transform);
        /*gameObject.transform.localEulerAngles = new Vector3(
            gameObject.transform.localEulerAngles.x,
            point.y + 180,
            gameObject.transform.localEulerAngles.z
        );*/
    }
}
