using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

public class VirtualPet : MonoBehaviour
{
    public int hunger = 100;
    public int health = 100;
    public int happiness = 100;
    public int cleanliness = 100;
    public bool tired {get;set;}
    public bool sleep {get;set;}
    public bool dirty {get;set;}
    float resetTimer;
    int age, maxAge;
    float hungerTimer, playTimer;
    public PetType type;
    string [] petNames= {"Owliver", "Blueberry","Lenny", "Penny", "Jenny", "Zach"};
    VirtualPetManager petKeeper;

    private void PetInit(PetType petClass)
    {
        petKeeper = VirtualPetManager.petManager;

        //switch to set pet class data -- 
        switch(petClass){
            case PetType.blueWhale:
                happiness = 50;
                int i;
                this.gameObject.name = petNames[i = UnityEngine.Random.Range(0,petNames.Length - 1)];
                break;
            default:
            type = PetType.ghost;
            break;
        }
    }
    public void OnEnable(){
        VirtualPetManager.petManager.RegisterPet(this);
    }
    public void OnDisable(){
        VirtualPetManager.petManager.UnRegisterPet(this);
    }
    private void Update()
    {
        hungerTimer += Time.deltaTime;
        resetTimer += Time.fixedDeltaTime;
        
        if(resetTimer >= 897f){
            age++;
            resetTimer = 0;
        }
        if(hungerTimer >= 3f){
            hungerTimer = 0;
            // Simulate hunger over time
            if(hunger > 0) hunger -= 1;
            
            if (hunger <= 0) {
                hunger = 0;
            } else
            // Decrease health if hunger is low
            if (hunger < 20){
                health -= 1;
                if(health <= 0){
                    Dead();
                    return;
                }
            }
        }
        if(petKeeper.nightTime){
            if(hunger < 80){
                happiness -= 3;
            } 
        }

    }

    private void Dead()
    {
        health = 0;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(.1f);
        // Death Animation
        // Garbage Collection
        Debug.Log($"The {type} of pet died");
    }

    public void Feed()
    {
        hunger += 20;
        if (hunger > 100) hunger = 100;
    }

    public void Play()
    {
        happiness += 20;
        if (happiness > 100) happiness = 100;
        cleanliness -= 5; // Playing decreases cleanliness a bit
    }

    public void RestClean()
    {
        health += 20;
        if (health > 100) health = 100;
        cleanliness += 20;
        if (cleanliness > 100) cleanliness = 100;
    }


}

public enum PetType {
    blueWhale,
    manatee,
    narwhal,
    penguin,
    owl,
    ghost
    //Owliver, Blueberry, Lenny, Penny, Jenny, Zach
}