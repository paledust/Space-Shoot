using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Chase : Task {
	private GameObject taskObject;
	private EnemyBoss enemyBoss{
		get{
			return taskObject.GetComponent<EnemyBoss>();
		}
	}
	public Task_Chase(GameObject m_taskObject)
	{
		Status = TaskStatus.Detached;
		taskObject = m_taskObject;
	}
	override protected void Init()
	{
		Debug.Log("CHASING TASK INITIALIZED");
		enemyBoss.SetMove(true);
	}
}
