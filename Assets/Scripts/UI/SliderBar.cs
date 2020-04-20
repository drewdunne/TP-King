using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{

    public Slider slider;

    void Start()
    {
    }
    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetValue(float currentValue)
    {
        slider.value = currentValue;
    }

    public void Refill()
    {
        slider.value = slider.maxValue;
    }

}

