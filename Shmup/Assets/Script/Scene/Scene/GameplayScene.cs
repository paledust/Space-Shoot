using UnityEngine;
using System.Collections;
public class GameplayScene : Scene<TransitionData> {
    int DestroyedEnemy;
	[SerializeField] Transform PlayerSpawnPos;
	[SerializeField] Transform BossSpawnPos;
	GameObject Boss;
	EnemyManager enemyManager;
	internal override void OnEnter(TransitionData data){
		Service.eventManager.ClearList();
		SetPlayer(PlayerSpawnPos);
		Boss = CreateBoss(BossSpawnPos);
		if(enemyManager == null){
			enemyManager = CreateEnemyManager().GetComponent<EnemyManager>();
		}
		Service.enemyManager = enemyManager;
		Camera.main.GetComponent<CameraBehavior>().SetFollowTrans(Service.player.transform);
		Service.eventManager.Register<BossDie>(BossDie_LoadScene);
		Service.eventManager.Register<EnemyDestroy>(PlusEnemy);
		Debug.Log("Enter Gameplay");
	}
	internal override void OnExit(){
		Destroy(Boss);
		enemyManager.DestroyThemAll();

		Debug.Log("Exit Gameplay");
	}
	GameObject CreateBoss(Transform BossSpawnTrans){
		GameObject Boss = Instantiate(Service.prefebObj.Boss, BossSpawnTrans.localPosition, BossSpawnTrans.rotation) as GameObject;
		Boss.transform.SetParent(transform, false);
		return Boss;
	}
	
	private IEnumerator countDown(){
		for (var i = 15; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
        }
		Service.sceneManager.Swap<KillScreen>(new TransitionData(DestroyedEnemy));
		yield return null;
	}
	void BossDie_LoadScene(Event e){
		StartCoroutine(countDown());
	}
	public void PlusEnemy(Event e){
		DestroyedEnemy ++;
	}

}
