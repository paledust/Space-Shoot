using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Appear : Task {
	private GameObject taskObject;
	private Transform taskTransform{
		get{
			return taskObject.transform;
		}
	}
	public Task_Appear(GameObject m_taskObject)
	{
		Status = TaskStatus.Detached;
		taskObject = m_taskObject;
	}
	override protected void Init()
	{
		Debug.Log("APPEARING TASK INITIALIZED");
		taskTransform.localScale = Vector3.one * 0.1f;
	}

	override internal void TUpdate()
	{

		Debug.Log("APPEARING TASK WORKING");
		taskTransform.localScale += Vector3.one * 0.01f;
		if(taskTransform.localScale.x >= 1)
		{
			taskTransform.localScale = Vector3.one;
			SetStatus(TaskStatus.Success);
		}
	}
}
