using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCareTaker : MonoBehaviour
{
    public VirtualPetManager manager;
    
    public void FeedSelectedPet(){
        //feed the selected pet
        manager.currentPet.Feed();
    }
    public void PlayWithSelectedPet(){
        //play with the selected pet
        manager.currentPet.Play();
    }
    public void CleanSelectedPet(){
        //Clean or Rest Selected pet
        manager.currentPet.RestClean();
    }
    public void SelectNextPet(){
        //Select the Next Available Pet
        manager.ChangeSelectedPet(1);
    }
    public void SelectPreviousPet(){
        //Select the Previous or last Available Pet
        manager.ChangeSelectedPet(-1);
    }
}
