using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class UIKeyCap : MonoBehaviour
{
    public Image imgKeycap;
    public TextMeshProUGUI txtKeyCap;

    public Color normalColor;
    public Color pressedColor;

    public void SetKey(KeyCode _code)
    {
        txtKeyCap.text = _code.ToString().ToUpper();
    }

    public void SetKey(string _key)
    {
        txtKeyCap.text = _key.ToUpper();
    }

    public void SetNormalColor()
    {
        imgKeycap.color = normalColor;
    }

    public void SetPressedColor()
    {
        imgKeycap.color = pressedColor;
    }
}
