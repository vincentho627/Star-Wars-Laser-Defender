using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{

    public Slider slider;

    private void Start()
    {
        if (FindObjectOfType<Player>() != null)
        {
            slider.maxValue = FindObjectOfType<Player>().GetHealth();
            slider.value = slider.maxValue;
        } 
    }

    public void SetHealthSlider(float health)
    {
        slider.value = health;
      
    }

    public void ResetHealthSlider()
    {
        slider.value = slider.maxValue;
    }

    public void TakeDamageSlider(int health)
    {
        slider.value -= health;
    }

    public float GetMaxHealthSlider()
    {
        return slider.maxValue;
    }
    
}
