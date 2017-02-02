using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trace : EnemyBase {
	private Quaternion originalRotation;
	public float FollowSpeed;
	
	void Start()
	{
		base.Start();
		
		moveSpeed = Random.Range(0.5f,2.0f);
		originalRotation = transform.rotation;
	}

	override protected void ColorInitial()
	{
		ShipColor = ColorChoose.ColorLibrary[(int)Random.Range(0,ColorChoose.ColorLibrary.Length)];

		GetComponent<SpriteRenderer>().color = ShipColor;
		GetComponent<TrailRenderer>().startColor = ShipColor;
		GetComponent<TrailRenderer>().endColor = ShipColor;
	}

	//Enemy will copy how the player "control" the ship with different initial speed.
	override protected void Move()
	{
		Vector3 playerVelocity = player.GetComponent<Control>().getVelocity();
		velocity = (originalRotation * playerVelocity.normalized).normalized * playerVelocity.magnitude * moveSpeed;
		transform.position += velocity;
	}

	override protected void rotate()
	{
		transform.rotation = player.transform.rotation * originalRotation;
	}
}
