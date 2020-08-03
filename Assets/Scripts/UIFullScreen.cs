using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFullScreen : MonoBehaviour
{
    public int designWidth = 960;//开发时分辨率宽
    public int designHeight = 640;//开发时分辨率高

    void Start () {
        int width = Screen.width;
        int height = Screen.height;
        float s1 = (float)designWidth / designHeight;
        float s2 = (float)width / height;
        if (s1 < s2)
            designWidth = Mathf.FloorToInt(designHeight * s2);
        else if (s1 > s2)
            designHeight = Mathf.FloorToInt(designWidth / s2);

        //float contentScale = (float)designWidth / (float)width;
        RectTransform rectTransform = transform as RectTransform;
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(designWidth, designHeight);
        }
    }

}
