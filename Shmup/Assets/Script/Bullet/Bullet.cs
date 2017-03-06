using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float damage;
	// Use this for initialization
	void Start () {
		Destroy(gameObject, 3.0f);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.GetComponent<EnemyBase>().ApplyDamage(damage);
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
