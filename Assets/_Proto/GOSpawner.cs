using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
public class GOSpawner : MonoBehaviour
{
    public GameObject spawned;
    public float coolDown;
    public bool spawning;
    public bool singleSpawn;
    int currentCount;
    public int maxCount = 1;
    private bool makeNewPet;
    public bool bombEnemy;
    public List<Transform> waypointList = new List<Transform>();


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
        if(makeNewPet)MakeNewPet(offset);
        if(bombEnemy)MakeBombEnemy(offset);
        currentCount ++;

        yield return new WaitForSeconds(coolDown);
        spawning = false;
    }

    private void MakeBombEnemy(Vector3 offset)
    {
        Vector3 newOffset = new Vector3(offset.x, 0f, offset.y);
        GameObject obj = Instantiate(spawned, transform.position + newOffset, transform.rotation);
        BombCopNPCController newNPC = obj.GetComponent<BombCopNPCController>();
        newNPC.SetupAI(true, waypointList);
        newNPC.GetComponent<Damageable>().Remortalize();
    }

    private void MakeNewPet(Vector3 offset)
    {
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
    }
}