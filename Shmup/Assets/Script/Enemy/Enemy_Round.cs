using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Round : EnemyBase {
	public float detectRange;
	public Vector2 CirclingRange;
	[SerializeField] private float CirclingRadius;
	private float distanceToPlayer;
	// Use this for initialization
	void Start () {
		base.Start();
		CirclingRadius = Random.Range(CirclingRange.x, CirclingRange.y);
	}

	override protected void Behave()
	{
		distanceToPlayer = (player.transform.position - transform.position).magnitude;
		if(distanceToPlayer <= detectRange)
		{
			Circling(CirclingRadius);
		}

		velocity *= 0.99f;
		transform.position += velocity;
	}

	//Enemy will move towards player
}
