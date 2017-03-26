using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Fire : Task {
	private GameObject taskObject;
	private EnemyBoss enemyBoss{
		get{
			return taskObject.GetComponent<EnemyBoss>();
		}
	}
	public Task_Fire(GameObject m_taskObject)
	{
		Status = TaskStatus.Detached;
		taskObject = m_taskObject;
	}
	override protected void Init()
	{
		Debug.Log("FIRING TASK INITIALIZED");
		enemyBoss.ifShoot = true;
	}
}
