using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Manager: MonoBehaviour {
	public List<Task> tasks;
	void Start()
	{
		int i = 0;
		foreach (Task task in tasks)
		{
			if(task.ifFinished) 
				HandleCompletion(task,i);
			else 
				task.SetStatus(Task.TaskStatus.Pending);
		}
	}
	void Update()
	{
		TaskHandle();
		int i = 0;
		foreach (Task task in tasks)
		{
			if(task.ifPending) 
				task.SetStatus(Task.TaskStatus.Working);

			if(task.ifFinished) 
				HandleCompletion(task,i);
			else
			{
				task.TUpdate();
				if(task.ifFinished) HandleCompletion(task,i);
			}
			i++;
		}
	}

	void HandleCompletion(Task task, int index)
	{
		if(task.NextTask != null && task.ifSuccess)
			AddTask(task.NextTask);
		tasks.RemoveAt(index);
		task.SetStatus(Task.TaskStatus.Detached);
	}

	void AddTask(Task task)
	{
		Debug.Log("Add Task");
		Debug.Assert(task != null);
		Debug.Assert(task.ifDetached);
		tasks.Add(task);
		task.SetStatus(Task.TaskStatus.Pending);
	} 

	protected void TaskHandle()
	{
		if(GetComponent<EnemyBoss>().health <= GetComponent<EnemyBoss>().MAXHEALTH * 0.5f && GetComponent<Task_Fire>().ifDetached)
		{
			Debug.Log("Try Add Fire");
			AddTask(GetComponent<Task_Fire>());
		}

		if(GetComponent<EnemyBoss>().health <= GetComponent<EnemyBoss>().MAXHEALTH * 0.15f && GetComponent<Task_Chase>().ifDetached)
		{
			Debug.Log("Try Add Chase");
			AddTask(GetComponent<Task_Chase>());
		}
	}
}
