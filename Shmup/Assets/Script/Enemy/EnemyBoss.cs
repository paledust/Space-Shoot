using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase {
	public float detectRange = 30.0f;
	public float MAXHEALTH = 100;
	public bool ifShoot = false;
	// // Use this for initialization
	protected void Start()
	{
		base.Start();
		SetMove(false);
	}

	protected void Update () {
		base.Update();
		if(ifShoot)
			ShootBullet();
	}
	override protected void Behave()
	{
		float distanceToPlayer = (player.transform.position - transform.position).magnitude;
		if(distanceToPlayer <= detectRange)
		{
			Debug.Log("MOVE");
			TowardPlayer(0.8f);
		}

		//velocity *= 0.99f;
		transform.position -= velocity;
	}

	public void SetMove(bool _ifMove)
	{
		ifMove = _ifMove;
	}
}
