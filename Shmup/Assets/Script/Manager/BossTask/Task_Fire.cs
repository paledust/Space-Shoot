using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Fire : Task {
	override protected void Init()
	{
		Debug.Log("FIRING TASK INITIALIZED");
		GetComponent<EnemyBoss>().ifShoot = true;
	}
}
