using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Task_Behavior : MonoBehaviour {
	private Task_Manager taskManager;
	void Start()
	{
		if(!GetComponent<Task_Manager>())
		{
			gameObject.AddComponent<Task_Manager>();
		}
		taskManager = GetComponent<Task_Manager>();
		InitalTask();
	}
	void Update()
	{
		TaskHandle();
	} 
	// Use this for initialization
	protected void TaskHandle()
	{
		if(GetComponent<EnemyBoss>().health <= GetComponent<EnemyBoss>().MAXHEALTH * 0.5f && GetComponent<Task_Fire>().ifDetached)
		{
			Debug.Log("Try Add Fire");
			taskManager.AddTask(GetComponent<Task_Fire>());
		}

		if(GetComponent<EnemyBoss>().health <= GetComponent<EnemyBoss>().MAXHEALTH * 0.15f && GetComponent<Task_Chase>().ifDetached)
		{
			Debug.Log("Try Add Chase");
			taskManager.AddTask(GetComponent<Task_Chase>());
		}
	}
	protected void InitalTask()
	{
		if(GetComponent<Task_Appear>().ifDetached)
		{
			Debug.Log("Try Add Appearance");
			taskManager.AddTask(GetComponent<Task_Appear>());
		}
	}
}
