using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

public class VirtualPet : MonoBehaviour
{
    public int hunger, health, happiness, cleanliness;
    public int maxHunger {get; private set;}
    public int maxHealth {get; private set;}
    public int maxHappiness {get; private set;}
    public int maxCleanliness {get; private set;}
    public float speed, range, maxDistance;
    Vector2 wayPoint;
    public bool alive {get; private set;}
    public bool tired {get; private set;}
    public bool sleep {get; private set;}
    public bool dirty {get; private set;}
    float resetTimer;
    public int age, maxAge;
    float hungerTimer, playTimer;
    public PetType type;
    public string [] petNames= {"Owliver", "Blueberry","Lenny", "Penny", "Jenny", "Zach"};
    VirtualPetManager petKeeper;
    public AudioClip feedSFX, playSFX, cleanSFX;
    public GameObject deathVFX;
    public VirtualPetStatBar LifeGuage; 

    public void PetInit(PetType petClass)
    {
        petKeeper = VirtualPetManager.petManager;

        //switch to set pet class data -- 
        switch(petClass){
            case PetType.blueWhale:
                SetLimitPetData(94, 51, 60, 52);
                break;
            case PetType.ghost:
                SetLimitPetData(38, 88, 98, 40);
                break;
            case PetType.narwhal:
                SetLimitPetData(50, 35, 27, 62);
                break;
            case PetType.owl:
                SetLimitPetData(20, 78, 66, 31);
                break;
            case PetType.penguin:
                SetLimitPetData(39, 19, 40, 65);
                break;
            default:
                break;
        }
        type = petClass;
        int i;
        this.gameObject.name = petNames[i = UnityEngine.Random.Range(0,petNames.Length - 1)];
        LifeGuage = GetComponentInChildren<VirtualPetStatBar>();
        GetComponentInChildren<VirtualGraphicsAssistant>().AssignPetGraphics(type);
        SetDestination();
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
        resetTimer += Time.deltaTime;
        playTimer += Time.deltaTime;
        
        if(resetTimer >= 897f){
            age++;
            resetTimer = 0;
        }
        if(hungerTimer >= 3f){
            hungerTimer = 0;
            // Simulate hunger over time
            hunger -= 1;
            
            if (hunger <= 0) {
                hunger = 0;
            }
            // Decrease health if hunger is low
            if (hunger < 20){
                health -= 1;
                if(health <= 0){
                    Dead();
                    return;
                }
            }
            if(playTimer >= 20f){
                playTimer = 0;
                happiness /= 2; 
            }
        }
        if(petKeeper.nightTime){
            if(hunger < 80){
                happiness -= 3;
            } 
        }
        WanderMovement();
        if(LifeGuage != null) LifeGuage.UpdateBar(health, maxHealth);

    }
    private void Dead()
    {
        health = 0;
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        Debug.Log($"The {type} of pet died");
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
        Instantiate(deathVFX, transform.position, transform.rotation);
        VirtualPetManager.petManager.ButtonSound();
    }
    public void Feed()
    {
        hunger += 15;
        if (hunger > 100) hunger = 100;
        cleanliness -= 5;
    }
    public void Play()
    {
        happiness += 6;
        if (happiness > 100) happiness = 100;
        cleanliness -= 15; // Playing decreases cleanliness a bit
        playTimer = 0;
    }
    public void RestClean()
    {
        health += 20;
        if (health > 100) health = 100;
        cleanliness += 20;
        if (cleanliness > 100) cleanliness = 100;
        if (happiness > 100) happiness = 100; else happiness += 15;
    }
    public void SetPetData(int hun, int hel, int hap, int cln){
        hunger = hun;
        health = hel;
        happiness = hap;
        cleanliness = cln;
    }
    public void SetLimitPetData(int hun, int hel, int hap, int cln){
        maxHunger = hun;
        maxHealth = hel;
        maxHappiness = hap;
        maxCleanliness = cln;
        SetPetData(maxHunger, maxHealth, maxHappiness, maxCleanliness);
    }
    void SetDestination(){
        wayPoint = new Vector2(UnityEngine.Random.Range(maxDistance, -maxDistance),UnityEngine.Random.Range(maxDistance, -maxDistance));
    }
    void WanderMovement(){
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, wayPoint) < range){
            SetDestination();
        }
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