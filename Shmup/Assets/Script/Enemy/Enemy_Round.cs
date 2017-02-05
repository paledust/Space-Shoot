using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Round : EnemyBase {
	public float detectRange;
	public Vector2 CirclingRange;
	[SerializeField] private float circlingRadius;
	private float distanceToPlayer;
	// Use this for initialization
	void Start () {
		base.Start();
	}

	override protected void Move()
	{
		distanceToPlayer = (player.transform.position - transform.position).magnitude;
		if(distanceToPlayer <= detectRange)
		{
			Circling();
		}

		velocity *= 0.99f;
		transform.position += velocity;
	}
	override protected void ColorInitial()
	{
		ShipColor = ColorChoose.ColorLibrary[(int)Random.Range(0,ColorChoose.ColorLibrary.Length)];

		GetComponent<SpriteRenderer>().color = ShipColor;
		GetComponent<TrailRenderer>().startColor = ShipColor;
		GetComponent<TrailRenderer>().endColor = ShipColor;
	}

	override protected void MoveInitial()
	{
		circlingRadius = Random.Range(CirclingRange.x, CirclingRange.y);
	}

	//Enemy will move towards player
	void Circling()
	{
		Vector3 toPlayer = player.transform.position - transform.position;
		velocity = (toPlayer + Quaternion.Euler(0,0,90) * toPlayer.normalized * circlingRadius).normalized * moveSpeed;
	}
}
