using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Follow : EnemyBase {
	private Color32 ShipColor;
	private GameObject player;
	private int direction;
	
	void Start()
	{
		base.Start();
		ShipColor = ColorChoose.ColorLibrary[(int)Random.Range(0,ColorChoose.ColorLibrary.Length)];
		GetComponent<SpriteRenderer>().color = ShipColor;

		GetComponent<TrailRenderer>().startColor = ShipColor;
		GetComponent<TrailRenderer>().endColor = ShipColor;
		player = GameObject.Find("SpaceShip");
		direction = (int)Random.Range(0,2)*2-1;
	}

	override protected void Move()
	{
		transform.rotation = player.transform.rotation;
		transform.position += player.GetComponent<Control>().getVelocity() * direction;
	}
}
