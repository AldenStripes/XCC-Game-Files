using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;
    private Slider slider;
    public Color colorSelection;
    
    // I replaced Start with Awake for performance reasons
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        float fillValue = playerHealth.damageTaken / 30f;
        slider.value = fillValue;
        if (fillValue <= slider.maxValue / 3)
        {
            fillImage.color = Color.white;
        }
        else if (fillValue > slider.maxValue / 3 && fillValue != slider.maxValue)
        {
            fillImage.color = Color.red;
        }
        else if (fillValue == slider.maxValue)
        {
            //fillImage.color = colorSelection;
        }
    }
}
