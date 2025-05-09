using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class BombController : MonoBehaviour
{
    public int damage = 10;
    public float radius = 3f;
    public float radiusMultiplier = 1f;
    int stackCount;
    public float delay = 2f;
    float force = 400f;
    float fuseTimer, maxFuseTime;
    bool player;
    public bool hasExploded{get; private set;}
    public GameObject radiusVFX;
    public GameObject explodedVFX;
    CinemachineImpulseSource impluse;
    public Image bombTime;
    public TextMeshProUGUI bombCount;
    public BombType type;
    public DetonatorType detonator;
    SphereCollider bombChecker;
    bool activated;
    private bool isProximity, isRemote, isLink;
    public AudioClip bombSFX;
    private float explosionDelay;

    public static event Action<BombController> onBombDestroy;

    void OnEnable()
    {
        BomberPlayerController.onDetonateAllBombs += Detonate;

    }
    void OnDisable()
    {
        BomberPlayerController.onDetonateAllBombs -= Detonate;
    }
    void Update()
    {
        if(activated) Countdown();
        if(radiusVFX != null) radiusVFX.transform.localScale = new Vector3(radius, radius, radius);
        BombUIUpdate();
    }

    private void BombUIUpdate()
    {
        if(bombTime != null){
            bombTime.gameObject.SetActive(fuseTimer > 0);
            bombTime.fillAmount = fuseTimer / maxFuseTime;
        }

        if(bombCount != null){ 
            if(stackCount <= 0){
                bombCount.gameObject.SetActive(false);
            }else {
                bombCount.gameObject.SetActive(true);
            }
            bombCount.text = stackCount.ToString();
        }
    }

    private void Countdown()
    {
        fuseTimer -= Time.deltaTime;
        if (fuseTimer <= 0f && !hasExploded)
        {
            hasExploded = true;
            StartCoroutine(Explode());
            return;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<BombInventoryController>(out BombInventoryController playerController))
        {
            playerController.SetStackTarget(this);
        }
        if(other.tag == "CanBoom" && isProximity){
            hasExploded = true;
            StartCoroutine(Explode());
            return; 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<BombInventoryController>(out BombInventoryController playerController))
        playerController.SetStackTarget(null);
    }
    public void Stack(){
        fuseTimer = delay;
        stackCount++;
    }
    void Detonate(){
        if(isRemote){
            hasExploded = true;
            StartCoroutine(Explode());
            return;
        }
    }
    public void LocalDetonate(){
        hasExploded = true;
        explosionDelay = 1f;
        StartCoroutine(Explode());
        return;
    }
    public void LinkDetonate(){
        if(isLink){
            hasExploded = true;
            StartCoroutine(Explode());
            return;
        }
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            player = nearbyObject.transform.gameObject.tag == "Player";
            if(player){
                Transform foundPlayer = nearbyObject.transform;
                Debug.Log("Player Explode Up");
                foundPlayer.gameObject.GetComponent<BomberPlayerController>().BombJump(damage);
            }
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
           //Damage
            if (rb != null)
            {
                if(rb.TryGetComponent<BombController>(out BombController nearbyBomb))
                {
                    if(!nearbyBomb.hasExploded)nearbyBomb.LocalDetonate();
                }
                if(rb.gameObject.TryGetComponent<Damageable>(out Damageable health)){
                    health.TakeDamage(damage);
                }
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
        //Audio Clip
        if(explodedVFX != null) Instantiate(explodedVFX, transform.position, transform.rotation);
        float newForce = force / 1000;
        if(impluse != null)impluse.GenerateImpulse(newForce);
        yield return new WaitForSeconds(.01f);
        if(bombSFX != null) AudioManager.instance.PlaySFXClip(bombSFX);
        onBombDestroy?.Invoke(this);
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public void SetupBomb(DetonatorType detonatorType, BombType bombType){
        detonator = detonatorType;
        type = bombType;
        impluse = GetComponent<CinemachineImpulseSource>();
        bombChecker = GetComponent<SphereCollider>();
        if(bombChecker!= null) bombChecker.isTrigger = true;
        switch (detonator)
        {
            case DetonatorType.Fuse:
            fuseTimer = delay;
            maxFuseTime = fuseTimer;
            activated = true;
            Debug.Log("Handling a fuse detonator.");
            break;
            
            case DetonatorType.Remote:
            isRemote = true;
            Debug.Log("Handling a remote detonator.");
            break;
            
            case DetonatorType.Link:
            isLink = true;
            Debug.Log("Handling a link detonator.");
            break;
            
            case DetonatorType.Proximity:
            isProximity = true;
            Debug.Log("Handling a proximity detonator.");
            break;
            
            default:
            Debug.Log("Unknown detonator type.");
            break;
        }

    }
}
public enum DetonatorType{
    Fuse,
    Remote,
    Link,
    Proximity
}
