using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Appear : Task {
	override protected void Init()
	{
		Debug.Log("APPEARING TASK INITIALIZED");
		transform.localScale = Vector3.one * 0.1f;
	}

	override internal void TUpdate()
	{
		Debug.Log("APPEARING TASK WORKING");
		transform.localScale += Vector3.one * 0.1f;
		if(transform.localScale.x >= 1)
		{
			transform.localScale = Vector3.one;
			SetStatus(TaskStatus.Success);
		}
	}
}
