using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Spawn : Task {
	private float timer;
	public float SpawnTime = 1.0f;
	override protected void Init()
	{
		CreateEnmey bossEvent = new CreateEnmey();
		bossEvent._transform = gameObject.transform;
		EventManager.Instance.Fire(bossEvent);
		Debug.Log("SPAWNING TASK INITIALIZED");
	}

	override internal void TUpdate()
	{
		timer += Time.deltaTime;
		if(timer >= SpawnTime)
		{
			timer = 0.0f;
			CreateEnmey bossEvent = new CreateEnmey();
			bossEvent._transform = gameObject.transform;
			EventManager.Instance.Fire(bossEvent);
			Debug.Log("Create Enemy Wave");
		}
	}
}
