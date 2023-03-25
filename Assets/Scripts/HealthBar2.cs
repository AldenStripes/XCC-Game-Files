using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar2 : MonoBehaviour
{
    public Player2Health player2Health;
    public Image fillImage;
    private Slider slider;
    public Color colorSelection;

    // I replaced Start with Awake for performance reasons
    void Awake()
    {
        slider = GameObject.Find("Slider (1)").GetComponent<Slider>();
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

        float fillValue = player2Health.damageTaken / 30f;
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
