using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Task_Behavior : MonoBehaviour {
	private Task_Appear taskAppear;
	private Task_Spawn taskSpawn;
	private Task_Chase taskChase;
	private Task_Fire taskFire;
	private Task_Manager taskManager;
	void Start()
	{
		InitalTask();
	}
	void Update()
	{
		TaskHandle();
	} 
	// Use this for initialization
	protected void TaskHandle()
	{
		if(GetComponent<EnemyBoss>().health <= GetComponent<EnemyBoss>().MAXHEALTH * 0.5f && taskFire.ifDetached)
		{
			Debug.Log("Try Add Fire");
			taskManager.AddTask(taskFire);
		}

		if(GetComponent<EnemyBoss>().health <= GetComponent<EnemyBoss>().MAXHEALTH * 0.15f && taskChase.ifDetached)
		{
			Debug.Log("Try Add Chase");
			taskManager.AddTask(taskChase);
		}
	}
	protected void InitalTask()
	{
		taskManager = GetComponent<EnemyBoss>().taskManager;
		taskAppear = new Task_Appear(gameObject);
		taskChase = new Task_Chase(gameObject);
		taskFire = new Task_Fire(gameObject);
		taskSpawn = new Task_Spawn(gameObject,10.0f);

		taskAppear.Then(taskSpawn);
		taskManager.AddTask(taskAppear);
	}
}
