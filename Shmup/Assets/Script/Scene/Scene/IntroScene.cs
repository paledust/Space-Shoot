using System.Collections;
using UnityEngine;

public class IntroScene : Scene<TransitionData> {
	public Transform PlayerSpawnLocation;
	public Transform StartZoneLocation;
	public Transform EnemySpawnTrans;
	public GameObject ChangeColor;
	EnemyManager enemyManager;
	internal override void OnEnter(TransitionData data){
		Service.eventManager.ClearList();
		SetPlayer(PlayerSpawnLocation);
		if(enemyManager == null){
			enemyManager = CreateEnemyManager().GetComponent<EnemyManager>();
		}
		Service.enemyManager = enemyManager;
		Service.enemyManager.createEnemy_Amount_Around_Pos(EnemyType.FearLight, EnemySpawnTrans, 10);
		Service.eventManager.Register<Enter_Level>(ENTER_GAME);
		Camera.main.GetComponent<CameraBehavior>().SetFollowTrans(Service.player.transform);

		ChangeColor = CreateStartZone();
		Debug.Log("Enter Intro");
	}
	internal override void OnExit(){
		Debug.Log(Service.enemyManager.transform.parent.name);
		enemyManager.DestroyThemAll();
		ChangeColor.GetComponent<ChangeColor>().Reset();
		Debug.Log("Exit Intro");
	}

	void ENTER_GAME(Event e){
		StartCoroutine(countDown());
	}

	GameObject CreateStartZone(){
		GameObject StartZone = Instantiate(Service.prefebObj.StartZone, StartZoneLocation.localPosition, StartZoneLocation.rotation) as GameObject;
		StartZone.transform.SetParent(transform, false);
		return StartZone;
	}

	private IEnumerator countDown(){
        for (var i = 5; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
        }
		Service.sceneManager.PushScene<GameplayScene>();
	}
}
public class Enter_Level: Event{}
