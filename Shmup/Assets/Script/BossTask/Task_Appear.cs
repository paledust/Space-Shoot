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
	private float timer = 0.0f;
	public Task_Appear(GameObject m_taskObject)
	{
		Status = TaskStatus.Detached;
		taskObject = m_taskObject;
	}
	override protected void Init()
	{
		timer = 0.0f;
		taskTransform.localScale = Vector3.one * 0.1f;
	}

	override internal void TUpdate()
	{
		timer += Time.deltaTime;
		taskTransform.localScale = Vector3.Lerp( Vector3.one * 0.1f ,Vector3.one, Easing.BackEaseOut(timer/2.0f));
		if(taskTransform.localScale.x >= 1)
		{
			taskTransform.localScale = Vector3.one;
			SetStatus(TaskStatus.Success);
		}
	}
}
