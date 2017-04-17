using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy_SpeedUp : Enemy_SmartEnemy {
	public float agility = 1.0f;
	protected override bool SEEKING_PLAYER(Enemy_SmartEnemy SmartEnemy)
	{
		TowardPlayer(agility);
		velocity = velocity.normalized * moveSpeed;
		transform.position += velocity;

		return true;
	}
}
