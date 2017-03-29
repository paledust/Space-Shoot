using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Round : EnemyBase {
	public float detectRange;
	public Vector2 CirclingRange;
	[SerializeField] private float CirclingRadius;
	private float distanceToPlayer;
	private float timer;
	private float tempSpeed;
	private float tempRadius;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		timer = 0.0f;
		CirclingRadius = Random.Range(CirclingRange.x, CirclingRange.y);
		tempSpeed = moveSpeed;
		tempRadius = CirclingRadius;
	}


	protected override void SearchingMove()
	{
		distanceToPlayer = (player.transform.position - transform.position).magnitude;
		if(distanceToPlayer <= detectRange)
		{
			timer += Time.deltaTime;
			moveSpeed = Mathf.Lerp(0.0f, tempSpeed, Easing.BackEaseIn(timer));
			CirclingRadius = Mathf.Lerp(150.0f, tempRadius, Easing.BackEaseIn(timer));

			Circling(CirclingRadius);
		}
		//timer = 0.0f;
		velocity *= 0.99f;
		transform.position += velocity;
	}
}
