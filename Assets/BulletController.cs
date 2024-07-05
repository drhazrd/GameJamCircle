using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;

	public float lifeTime;
	public AudioClip hitClip;
	public AudioClip fizzClip;
	public int damageToGive;
	public GameObject impactFX;
	public bool isPlayerProjectile;
	public bool isNPCProjectile;
	void Update () {
		transform.Translate(Vector3.forward * speed * Time.deltaTime);

		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0)
		{
			Destroy(gameObject);
		}
	}
	void OnCollisionEnter(Collision col)
	{
		Destroy(gameObject);
		Debug.Log($"{col.transform.name}");
    }
    
}