using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class NormalSlider : MonoBehaviour
{
    [Range(0f, 1f)]
    public float slider;

    public RectTransform rtContainer;
    public RectTransform rtSlider;
    public Image imgSlider;

    public Gradient gradient;

    float containerSize_x;

    
    protected IEnumerator Start()
    {
        yield return null;

        float _width = rtContainer.rect.width;
        float _height = rtContainer.rect.height;

        rtContainer.anchorMax = Vector2.zero;
        rtContainer.anchorMin = Vector2.zero;
        rtContainer.anchoredPosition = Vector2.up * 5f;
        rtContainer.pivot = Vector2.up * 0.5f;
        rtContainer.sizeDelta = new Vector2(_width, _height);
        containerSize_x = rtContainer.sizeDelta.x;
    }

    public void SetSlidervalue(float _sliderValue)
    {
        slider = _sliderValue;

        float _x = containerSize_x;
        rtSlider.offsetMax = new Vector2(-_x * Mathf.Lerp(1, 0f, slider), rtSlider.offsetMin.y);

        imgSlider.color = gradient.Evaluate(slider);
    }

    private void OnValidate()
    {
        SetSlidervalue(slider);
    }
}
