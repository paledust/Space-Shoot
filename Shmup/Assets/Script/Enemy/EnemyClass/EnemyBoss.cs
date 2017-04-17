using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy_FSM_Enemy {
	public float detectRange = 30.0f;
	public float MAXHEALTH = 100;
	public bool ifShoot = false;
	// // Use this for initialization
	override protected void Start()
	{
		base.Start();
		fsm.TransitionTo<BossState>();
		moveSpeed = 0.0f;
	}

	override protected void Update () {
		base.Update();
		if(ifShoot)
			ShootBullet();
	}
	internal override void OtherMove()
	{
		float distanceToPlayer = (Service.player.transform.position - transform.position).magnitude;
		if(distanceToPlayer <= detectRange)
		{
			TowardPlayer(0.8f);
		}
		transform.position -= velocity;
	}

	override public void RegistHandler()
	{}

	override public void UnregistHandler()
	{} 
	public float GetSpeed()
	{
		return moveSpeed = Random.Range(SpeedRange.x,SpeedRange.y);
	}
	public class BossState: EnemyState{
		public override void Update()
		{
			Context.OtherMove();
		}
	}
	void OnDestroy()
	{
		BossDie tempEvent = new BossDie();
		Service.eventManager.Fire(tempEvent);
	}
}
