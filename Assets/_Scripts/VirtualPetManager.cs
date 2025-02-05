using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VirtualPetManager : MonoBehaviour
{
    public static VirtualPetManager petManager;
    List<VirtualPet> pets = new List<VirtualPet>();
    public VirtualPet currentPet {get; private set;}
    VirtualCareTaker careTakerController;
    public GameObject petDataContainer;
    public TextMeshProUGUI petDataText;
    float timer;
    private int days;
    private int months;
    private int years;
    private float dayRate = 875f;
    public bool nightTime;

    private void Awake()
    {
        if (petManager == null)
        {
            petManager = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start(){
        careTakerController = GetComponentInChildren<VirtualCareTaker>();
        careTakerController.manager = this;
    }
    public void UpdateUI()
    {
        petDataText.text = $"Hunger: {currentPet.hunger} \nHealth: {currentPet.health} \nHappiness:{currentPet.happiness} \nCleanliness: {currentPet.cleanliness}";
    }
    void Update(){
        timer += Time.deltaTime;
        if(timer == dayRate / 2){
            nightTime = true;
        }
        if(timer >= dayRate){
            nightTime = !nightTime;
            days++;
            timer = 0;
            if(days > 30){
                days -= 30;
                months++;
                if(months > 12){
                    years++;
                    months = 0;
                }
            }
        }

        bool petsAvailable = pets.Count > 0;
        if(petsAvailable){
            UpdateUI();
        } 
        if(petDataContainer != null) petDataContainer.SetActive(petsAvailable);
    }
    public void RegisterPet(VirtualPet newPet){
        pets.Add(newPet);
        if(currentPet == null){
            currentPet = newPet;
        }
    }
    public void UnRegisterPet(VirtualPet oldPet){
        pets.Remove(oldPet);
        if(currentPet == oldPet){
            currentPet = pets[0] != null ? pets[0] : null;            
        }
    }
    public void RegisterController(VirtualCareTaker control){
        careTakerController = control;
    }
    public void ChangeSelectedPet(int p){
        if(pets.Count > 0){
            int petID = pets.IndexOf(currentPet);
            petID += p;
            if(petID < 0){
                petID = pets.Count - 1;
            } else if(petID > pets.Count - 1) {
                petID = 0;
            }
            currentPet = pets[petID];
        }
    }
    
}