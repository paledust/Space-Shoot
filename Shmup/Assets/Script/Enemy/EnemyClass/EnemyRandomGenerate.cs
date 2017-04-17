using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomGenerate : MonoBehaviour {
	// Update is called once per frame
	void Update()
	{
		if(Service.enemyManager.CountAll() == 0)
		{
			EnemyWaveDestroy waveDestroy = new EnemyWaveDestroy();
			Service.eventManager.Fire(waveDestroy);
		}
	}
}
