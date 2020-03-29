using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBars : MonoBehaviour
{
    public Slider slider;

    public void SetValue(float barValue)
    {
        slider.value = barValue;
    }

    public void SetMaxValue(float maxBarValue)
    {
        slider.maxValue = maxBarValue;
        //slider.value = maxBarValue;
        
    }
}
