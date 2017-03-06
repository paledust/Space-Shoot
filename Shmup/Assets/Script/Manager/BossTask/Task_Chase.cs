using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Chase : Task {
	override protected void Init()
	{
		Debug.Log("CHASING TASK INITIALIZED");
		GetComponent<EnemyBoss>().SetMove(true);
	}
}
