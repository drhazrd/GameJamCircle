using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCareTaker : MonoBehaviour
{
    public VirtualPetManager manager;
    
    public void FeedSelectedPet(){
        //feed the selected pet
        manager.currentPet.Feed();
        VirtualPetManager.petManager.ButtonSound();
    }
    public void PlayWithSelectedPet(){
        //play with the selected pet
        manager.currentPet.Play();
        VirtualPetManager.petManager.ButtonSound();
    }
    public void CleanSelectedPet(){
        //Clean or Rest Selected pet
        manager.currentPet.RestClean();
        VirtualPetManager.petManager.ButtonSound();
    }
    public void SelectNextPet(){
        //Select the Next Available Pet
        manager.ChangeSelectedPet(1);
        VirtualPetManager.petManager.ButtonSound();
    }
    public void SelectPreviousPet(){
        //Select the Previous or last Available Pet
        manager.ChangeSelectedPet(-1);
        VirtualPetManager.petManager.ButtonSound();
    }
}
