using UnityEngine;

public class Main : MonoBehaviour {
	void Awake()
	{
		Service.eventManager = new EventManager();

		Service.prefebObj = Resources.Load<PrefebObj>("Scene_Prefeb/LoadScene");
		Service.player = CreatePlayer();
		Service.sceneManager = new SceneManager<TransitionData>(gameObject, Service.prefebObj.Scenes);

		Service.sceneManager.PushScene<IntroScene>();
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.K) && Service.enemyManager != null)
		{
			Debug.Log(Service.enemyManager.transform.parent.name);
			Service.enemyManager.KillThemAll();
		}
	}
	protected GameObject CreatePlayer(){
		GameObject player = Instantiate(Service.prefebObj.player) as GameObject;
		player.transform.SetParent(transform, false);

		return player;
	}
}
