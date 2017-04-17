using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillScreen : Scene<TransitionData> {
	[SerializeField] Transform EnemySpawnTrans;
	EnemyManager enemyManager;
	internal override void OnEnter(TransitionData data){
		Service.eventManager.ClearList();
		data.EnemyKilled = 10;
		if(enemyManager == null){
			enemyManager = CreateEnemyManager().GetComponent<EnemyManager>();
		}
		Service.enemyManager = enemyManager;
		Service.enemyManager.createEnemy_Amount_Around_Pos(EnemyType.FearLight, EnemySpawnTrans, 10);
		Camera.main.GetComponent<CameraBehavior>().SetFollowTrans(Service.player.transform);
		Debug.Log(data.EnemyKilled);
		EnemyWaveDestroy tempEvent = new EnemyWaveDestroy();
		Debug.Log("Enter Kill");
	}
	internal override void OnExit(){
		enemyManager.DestroyThemAll();
		Debug.Log("Exit Kill");
	}
}
