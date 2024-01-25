using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagScript : MonoBehaviour
{
 private float siteX;
    private float siteY;
    private float siteZ;
    public Text textChild;

    public void setRelativePosition(float x, float y, float z) {
        siteX = x;
        siteY = y;
        siteZ = z;
        textChild.transform.position = new Vector3(x,y,z);
    }

    public void setText(string t) {
        textChild.text = t;
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
    void Start()
    {
        textChild = gameObject.GetComponentInChildren<Text>();        
    }
}
