using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trace : EnemyBase {
	private Quaternion originalRotation;
	
	protected override void Start()
	{
		base.Start();
		originalRotation = transform.rotation;
	}

	//Enemy will copy how the player "control" the ship with different initial speed.
	protected override void SearchingMove(){
		CopyMovement(originalRotation);
		
		transform.position += velocity;
	}
}
