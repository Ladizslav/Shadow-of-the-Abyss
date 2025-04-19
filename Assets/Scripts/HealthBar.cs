using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public Health healthComponent; // Reference na komponentu Health
    private float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        if (healthComponent != null)
        {
            healthSlider.maxValue = healthComponent.maxHealth;
            easeHealthSlider.maxValue = healthComponent.maxHealth;
            healthSlider.value = healthComponent.currentHealth;
            easeHealthSlider.value = healthComponent.currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthComponent != null)
        {
            float health = healthComponent.currentHealth;
            if (healthSlider.value != health)
            {
                healthSlider.value = health;
            }
            if (healthSlider.value != easeHealthSlider.value)
            {
                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
            }
        }
    }
}
