using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliders : MonoBehaviour
{ 
    public GameObject player;
    public GameObject sniper;
    public Slider playerBar;
    public Slider launchSlider;
    public float currentHealth;
    public float maxHealth;

    private float shotTime;
    private float fireRate;

    void Awake()
    {
        //player = GameObject.FindWithTag("Player");
        maxHealth = player.GetComponent<PlayerHealth>().maxHealth;
        fireRate = sniper.GetComponent<ShootingAtPlayer>().fireRate;
    }
    public void SliderValue(Slider slider, float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    private void Update() {
        shotTime = sniper.GetComponent<ShootingAtPlayer>().fireTimer;
        SliderValue(launchSlider, shotTime, fireRate);
        if(player != null){
            currentHealth = player.GetComponent<PlayerHealth>().health;
            SliderValue(playerBar, currentHealth, maxHealth);
        }
    }
}
