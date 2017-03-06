using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Manager: MonoBehaviour {
	public List<Task> tasks;
	void Start()
	{
		tasks = new List<Task>();
	}
	void Update()
	{
		int i = 0;
		foreach (Task task in tasks)
		{
			if(task.ifPending) task.SetStatus(Task.TaskStatus.Working);

			if(task.ifFinished) HandleCompletion(task,i);
			else
			{
				task.Update();
				if(task.ifFinished) HandleCompletion(task,i);
			}
			i++;
		}
	}

	void HandleCompletion(Task task, int index)
	{
		tasks.RemoveAt(index);
		task.SetStatus(Task.TaskStatus.Detached);
	}

	void AddTask(Task task)
	{
    	Debug.Assert(task != null);
    	Debug.Assert(task.ifDetached);
		tasks.Add(task);
		task.SetStatus(Task.TaskStatus.Pending);
	}
}
