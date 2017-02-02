using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Round : EnemyBase {
	public float detectRange;
	public float circlingRadius;
	public float followRange;
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
			if(distanceToPlayer <= circlingRadius)
			{
				circling();
			}
			else
			{
				moveToplayer();
			}
		}

	}
	override protected void ColorInitial()
	{
		ShipColor = ColorChoose.ColorLibrary[(int)Random.Range(0,ColorChoose.ColorLibrary.Length)];

		GetComponent<SpriteRenderer>().color = ShipColor;
		GetComponent<TrailRenderer>().startColor = ShipColor;
		GetComponent<TrailRenderer>().endColor = ShipColor;
	}
	override protected void rotate()
	{
		float rotationDegree = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		if(velocity.magnitude >= 0.01f)
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationDegree);
	}
	//Enemy will circle around the player
	void circling()
	{
		Vector3 toPlayer = player.transform.position - transform.position;
		velocity = Quaternion.Euler(0,0,90) * toPlayer.normalized * moveSpeed;

		transform.position += velocity;
	}
	//Enemy will move towards player
	void moveToplayer()
	{
		// Vector3 toPlayer = player.transform.position - transform.position;
		// velocity = (toPlayer + Quaternion.Euler(0,0,90) * toPlayer.normalized * circlingRadius).normalized * moveSpeed;

		// transform.position += velocity;
		float angle = 1000.0f * Time.deltaTime;
		velocity = new Vector3(Mathf.Sin(angle)*Mathf.Rad2Deg, Mathf.Cos(angle)*Mathf.Rad2Deg, 0.0f);

		transform.position += velocity;
	}

}
