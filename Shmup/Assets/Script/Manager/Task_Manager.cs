using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Manager: MonoBehaviour {
	public List<Task> tasks = new List<Task>();
	void Update()
	{
		for(int j = tasks.Count-1; j>= 0; j--)
		{
			Task task = tasks[j];
			if(task.ifPending) 
			{
				task.SetStatus(Task.TaskStatus.Working);
			}

			if(task.ifFinished) 
				HandleCompletion(task,j);
			else
			{
				task.TUpdate();
				if(task.ifFinished) HandleCompletion(task,j);
			}
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
		Debug.Assert(task != null);
		Debug.Assert(task.ifDetached);
		tasks.Add(task);
		task.SetStatus(Task.TaskStatus.Pending);
	} 
}
