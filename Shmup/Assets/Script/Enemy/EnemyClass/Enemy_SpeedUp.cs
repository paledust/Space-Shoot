﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpeedUp : Enemy_FSM_Enemy {
	[SerializeField] float Speed{
		get{
			return moveSpeed;
		}
		set{
			moveSpeed = value;
			if(moveSpeed > 2.0f)
			{
				moveSpeed = 2.0f;
			}
		}
	}
	[SerializeField] float agility = 1.0f;

	protected override void SearchingMove()	{
		TowardPlayer(agility);
		velocity = velocity.normalized * Speed;
		transform.position += velocity;
	}

	private void IncrementSpeed(float m_Speed)
	{
		Speed += m_Speed;
	}

	public void Handler_SpeedUp(Event e)
	{
		float addSpeed = 0.0f;
		EnemyDestroy enemyDestroy = e as EnemyDestroy;
		switch (enemyDestroy.enemyType)
		{
			case EnemyType.RoundCrazy:
				addSpeed = 0.05f;
				break;
			case EnemyType.RoundNormal:
				addSpeed = 0.1f;
				break;
			case EnemyType.SpeedUp:
				addSpeed = 0.3f;
				break;
			default:
				addSpeed = 0.05f;
				break;
		}
		
		IncrementSpeed(addSpeed);
	}

	override public void UnregistHandler()
	{
		Service.eventManager.Unregister<EnemyDestroy>(Handler_SpeedUp);
	}

	override public void RegistHandler()
	{
		Service.eventManager.Register<EnemyDestroy>(Handler_SpeedUp);
	}
}
