using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Manager:MonoBehaviour {
	public List<Task> tasks = new List<Task>();
	void Start()
	{

	}
	void Update()
	{
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

	public void AddTask(Task task)
	{
		Debug.Log("Add Task");
		Debug.Assert(task != null);
		Debug.Assert(task.ifDetached);
		Debug.Log(tasks);
		tasks.Add(task);
		task.SetStatus(Task.TaskStatus.Pending);
	} 
}
