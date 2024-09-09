using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holster : MonoBehaviour
{
    [SerializeField] int selectedWeapon = 0;
    public AudioClip swapSound;
    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if(Input.GetAxis("Mouse ScrollWheel") > 0f){
            WeaponUp();
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f){
            WeaponDown();
        }
        if (previousSelectedWeapon != selectedWeapon){
            SelectWeapon();
        }
    }
    void SelectWeapon(){
        int i = 0;
        foreach (Transform weapon in transform){
            
            if(i == selectedWeapon){
                weapon.gameObject.SetActive(true);
            }else{
                weapon.gameObject.SetActive(false);
            }
            
            i++;
        }
        if(swapSound != null) AudioManager.instance.PlaySFXClip(swapSound);
    }
    void WeaponUp(){
        if(selectedWeapon >= transform.childCount - 1)
            selectedWeapon = 0;
        else
            selectedWeapon ++;
    }
    void WeaponDown(){
        if(selectedWeapon <= 0)
            selectedWeapon = transform.childCount - 1;
        else
            selectedWeapon --;
    }
}
