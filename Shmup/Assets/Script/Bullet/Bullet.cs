using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float lifeTime;
	public float damage;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,lifeTime);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.GetComponent<EnemyBase>().ApplyDamage(damage);
		}
	}
}
