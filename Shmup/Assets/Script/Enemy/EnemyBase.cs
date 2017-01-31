using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
	protected float moveSpeed;
	public AudioClip destroySound;
	// Use this for initialization
	protected void Start () {
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	protected void Update () {
		Move();
	}

	virtual protected void Move()
	{
		
	}

	virtual protected void Rotate()
	{

	}

	virtual protected void ApplyDamage(float Damage)
	{

	}

	protected void HitSound()
	{
		GetComponent<AudioSource>().PlayOneShot(destroySound);
	}
	protected void Kill()
	{
		Destroy(gameObject);
	}


}
