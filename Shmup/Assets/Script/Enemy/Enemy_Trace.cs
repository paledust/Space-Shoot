using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trace : EnemyBase {
	private Quaternion originalRotation;
	
	void Start()
	{
		base.Start();
		originalRotation = transform.rotation;
	}

	//Enemy will copy how the player "control" the ship with different initial speed.
	override protected void Move()
	{
		CopyMovement(originalRotation);
		
		transform.position += velocity;
	}
}
