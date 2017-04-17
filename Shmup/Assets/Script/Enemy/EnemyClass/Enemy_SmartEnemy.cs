using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Enemy_SmartEnemy : EnemyBase {
	[SerializeField] float SeekingRadius = 20.0f;
	[SerializeField] float FleeRange = 50.0f;
	private float timer = 0.0f;
	protected Tree<Enemy_SmartEnemy> tree;
	protected bool ifChangingColor = false;
	// Use this for initialization
	protected override void Start () {
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();

		moveSpeed = Random.Range(SpeedRange.x,SpeedRange.y);
		GetComponent<AudioSource>().PlayOneShot(spawnSound);
		ColorInitial();
		MoveInitial();

		InitBehaviorTree();
	}
	virtual protected void InitBehaviorTree(){
		tree = new Tree<Enemy_SmartEnemy>(
			new Sequence<Enemy_SmartEnemy>(
				new Condition<Enemy_SmartEnemy>(IF_Alive),
				new Selector<Enemy_SmartEnemy>(
					new Sequence<Enemy_SmartEnemy>(
						new Condition<Enemy_SmartEnemy>(IF_OutRange),
						new Do<Enemy_SmartEnemy>(SEEKING_PLAYER)
					),
					new Sequence<Enemy_SmartEnemy>(
						new Condition<Enemy_SmartEnemy>(OverTime),
						new Selector<Enemy_SmartEnemy>(
							new Do<Enemy_SmartEnemy>(PRE_ATTACK),
							new Do<Enemy_SmartEnemy>(ATTACK)
						)
					),
					new Sequence<Enemy_SmartEnemy>(
						new Not<Enemy_SmartEnemy>(
							new Condition<Enemy_SmartEnemy>(IF_FleeOut)
						),
						new Do<Enemy_SmartEnemy>(FLEE)
					)
				)
			)
		);
	}
	// Update is called once per frame
	protected override void Update () {
		rotate();
		if(health <= 0.0f && !ifKill)
			Kill();
		if(ifChangingColor)
			ChangingColor();
		tree.Update(this);
	}
	protected bool IF_Alive(Enemy_SmartEnemy SmartEnemy){return SmartEnemy.health > 0.0f;}
	protected bool IF_OutRange(Enemy_SmartEnemy SmartEnemy){return DISTANCE_TO_PLAYER().magnitude >= SeekingRadius;}
	protected bool IF_FleeOut(Enemy_SmartEnemy SmartEnemy){
		ifChangingColor = false;
		return DISTANCE_TO_PLAYER().magnitude >= FleeRange;
	}
	protected virtual bool OverTime(Enemy_SmartEnemy SmartEnemy){
		timer += Time.deltaTime;
		if(timer >= 5.0f)
		{
			timer = 0.0f;
			return false;
		}
		return true;
	}
	protected virtual bool SEEKING_PLAYER(Enemy_SmartEnemy SmartEnemy){return false;}
	protected virtual bool PRE_ATTACK(Enemy_SmartEnemy SmartEnemy){
		ifChangingColor = true;
		return !ifChangingColor;
	}
	protected virtual bool ATTACK(Enemy_SmartEnemy SmartEnemy){
		Circling(150.0f);
		velocity *= 0.99f;
		transform.position += velocity;

		return true;
	}
	protected virtual bool FLEE(Enemy_SmartEnemy SmartEnemy){
		FleePlayer(3.0f);	
		transform.position += velocity;

		return true;
	}
	// protected class CountTime: Condition<Enemy_SmartEnemy>{
	// 	private float timer;

	// 	protected bool OverTime(Enemy_SmartEnemy SmartEnemy){
	// 		timer += Time.deltaTime;
	// 		if(timer >= 5.0f)
	// 		{
	// 			timer = 0.0f;
	// 			return false;
	// 		}
	// 		return true;
	// 	}
	// }
}
