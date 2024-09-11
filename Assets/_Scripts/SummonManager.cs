using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public static SummonManager summons;
    public List<Summon> activeSummons = new List<Summon>();
    public int summonResources;
    public GameObject[] summonPrefabs;
    public int summonPrefabID;

    void Start()
    {
        if (summons != null && SummonManager.summons != this){Destroy(this);}else if (SummonManager.summons == null){summons = this;}        
    }

    public void NewResources(){
        summonResources++;
    }
    public void NewSummon(Transform t, int id){
        summonPrefabID = id;
        if(summonResources > 0){
            Summon(t, summonPrefabs[summonPrefabID]);
            summonResources--;
        }
    }
    void Summon(Transform t, GameObject obj){
        GameObject createdSummon = Instantiate(obj, t.position, t.rotation);

    }
}
