using System.Collections;
using System;
using UnityEngine;

public class GOSpawner : MonoBehaviour
{
    public GameObject spawned;
    public float coolDown;
    public bool spawning;
    public bool singleSpawn;
    int currentCount;
    public int maxCount = 1;   
    void Update(){
        bool done = currentCount >= maxCount;
        if(!spawning && !done){
            StartCoroutine(SpawnProcess());
        } else return;
    }
    IEnumerator SpawnProcess(){
        spawning = true;
        float newOffsetX = UnityEngine.Random.Range(0, 4f);
        float newOffsetY = UnityEngine.Random.Range(0, 8f);
        Vector3 offset = new Vector3(newOffsetX, newOffsetY, 0);
        GameObject obj = Instantiate(spawned, transform.position + offset, transform.rotation);
        obj.AddComponent<Rigidbody2D>();
        obj.GetComponent<Rigidbody2D>().gravityScale = 0;
        obj.AddComponent<CircleCollider2D>();
        int typeCount = Enum.GetValues(typeof(PetType)).Length;
        VirtualPet newPet = obj.GetComponent<VirtualPet>();
        
        System.Random random = new System.Random();
        int randomIndex = random.Next(typeCount);

        // Get the random PetType value
        PetType randomPetType = (PetType)Enum.GetValues(typeof(PetType)).GetValue(randomIndex);

        // Initialize the pet with the random PetType value
        newPet.PetInit(randomPetType);
        newPet.speed = UnityEngine.Random.Range(1f, 3f);
        newPet.range = UnityEngine.Random.Range(0.5f, 3f);
        newPet.maxDistance = UnityEngine.Random.Range(3f, 10f);
        currentCount ++;

        yield return new WaitForSeconds(coolDown);
        spawning = false;
        Debug.Log($"Spawn {randomPetType}");
    }
}