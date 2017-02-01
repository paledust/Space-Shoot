using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trace : EnemyBase {
	private Color32 ShipColor;
	private GameObject player;
	private Quaternion originalRotation;

	public float FollowSpeed;
	
	void Start()
	{
		base.Start();
		ShipColor = ColorChoose.ColorLibrary[(int)Random.Range(0,ColorChoose.ColorLibrary.Length)];
		FollowSpeed = Random.Range(0.5f,2.0f);
		GetComponent<SpriteRenderer>().color = ShipColor;

		GetComponent<TrailRenderer>().startColor = ShipColor;
		GetComponent<TrailRenderer>().endColor = ShipColor;
		originalRotation = transform.rotation;
		player = GameObject.Find("SpaceShip");
	}

	override protected void Move()
	{
		Vector3 playerVelocity = player.GetComponent<Control>().getVelocity();
		transform.rotation = player.transform.rotation * originalRotation;
		transform.position += (originalRotation * playerVelocity.normalized).normalized * playerVelocity.magnitude * FollowSpeed;
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		Destroy(this);
	}
}
