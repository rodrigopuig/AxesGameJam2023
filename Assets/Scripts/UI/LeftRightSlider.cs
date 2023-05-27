using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LeftRightSlider : MonoBehaviour
{
    [Range(0f,1f)]
    public float slider;

    public RectTransform rtContainer;
    public RectTransform rtSlider;
    public Image imgSlider;

    public Gradient gradient;

    float containerSize_x;

    

    private void Awake()
    {
        
    }

    private IEnumerator Start()
    {
        yield return null;

        float _width = rtContainer.rect.width;
        float _height = rtContainer.rect.height;

        rtContainer.anchorMax = Vector2.right * 0.5f;
        rtContainer.anchorMin = Vector2.right * 0.5f;
        rtContainer.anchoredPosition = Vector2.up * 5f;
        rtContainer.sizeDelta = new Vector2(_width, _height);
        containerSize_x = rtContainer.sizeDelta.x;
    }

    public void SetSlidervalue(float _sliderValue)
    {
        slider = _sliderValue;

        float _x = containerSize_x;
        if (slider < 0.5f)
        {
            rtSlider.offsetMin = new Vector2(_x * 0.5f * Mathf.Lerp(0, 1f, slider / 0.5f), rtSlider.offsetMin.y);
            rtSlider.offsetMax = new Vector2(-_x * 0.5f, rtSlider.offsetMax.y);
        }
        else if (slider >= 0.5f)
        {
            rtSlider.offsetMin = new Vector2(_x * 0.5f, rtSlider.offsetMin.y);
            rtSlider.offsetMax = new Vector2(-_x * 0.5f * Mathf.Lerp(1, 0f, (slider - 0.5f) / 0.5f), rtSlider.offsetMax.y);
        }

        imgSlider.color = gradient.Evaluate(slider);
    }
}
