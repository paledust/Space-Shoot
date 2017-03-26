using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Spawn : Task {
	private GameObject taskObject;
	private float timer;
	public float SpawnTime = 1.0f;
	public Task_Spawn(GameObject m_taskObject, float m_spawnTime)
	{
		Status = TaskStatus.Detached;
		SpawnTime = m_spawnTime;
		taskObject = m_taskObject;
	}
	override protected void Init()
	{
		CreateEnmey bossEvent = new CreateEnmey();
		bossEvent._transform = taskObject.transform;
		EventManager.Instance.Fire(bossEvent);
	}

	override internal void TUpdate()
	{
		timer += Time.deltaTime;
		if(timer >= SpawnTime)
		{
			timer = 0.0f;
			CreateEnmey bossEvent = new CreateEnmey();
			bossEvent._transform = taskObject.transform;
			EventManager.Instance.Fire(bossEvent);
		}
	}
}
