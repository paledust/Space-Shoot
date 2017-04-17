using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType{
	RoundNormal,
	RoundCrazy,
	TraceNormal,
	TranceCrazy,
	SpeedUp,
	SmartEnemy_Round,
	FearLight,
	Boss
};

public class EnemyManager : MonoBehaviour {
	public GameObject enemyBasicPrefeb;
	public List<GameObject> enemyList  = new List<GameObject>();
	public Transform[] SpawnLocation;
	public int MaxSpawnNum;
	public float waveCooldown;

	public Sprite enemySprite_Circle;
	public Sprite enemySprite_Arrow;

	public float EnemyRatio;
	private int RoundNum;
	private int TraceNum;
	// Use this for initialization
	void Start () {
		enemySprite_Circle = Resources.Load<Sprite>("Image/Enemy_1");
		enemySprite_Arrow = Resources.Load<Sprite>("Image/SpaceShip");

		Service.eventManager.Register<EnemyWaveDestroy>(CreateWave);
		Service.eventManager.Register<CreateEnmey>(createWaveAtPos);
	}

	private Transform RandomSelectSpawnLocation()
	{
		return SpawnLocation[Random.Range(0,SpawnLocation.Length)];
	}

	private Transform SetSpawnLocation(Transform objectTransform)
	{
		return objectTransform;
	}
	//Create Types of Enemy
	public GameObject CreateEnemy(EnemyType enemyType, Transform enemyTransform)
	{
		GameObject _enemy = Instantiate(enemyBasicPrefeb, enemyTransform.position, enemyTransform.rotation) as GameObject;
		_enemy.name = "Enemy" + enemyType.ToString() + CountType(enemyType).ToString();
		
		//Choose which type of Enemy to spawn and add component to make enmey
		switch (enemyType)
		{
			case EnemyType.RoundNormal:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Arrow;
				Enemy_Round _round = _enemy.AddComponent<Enemy_Round>();
				_round.enemyType = EnemyType.RoundNormal;
				_round.SpeedRange = new Vector2(0.5f,2.0f);
				_round.CirclingRange = new Vector2(50,200);
				_round.detectRange = 20.0f;
				_round.health = 1.0f;
				_round.SearchingRange = 3.0f;

				break;
			case EnemyType.RoundCrazy:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Arrow;
				Enemy_Round _roundCrazy = _enemy.AddComponent<Enemy_Round>();
				_roundCrazy.enemyType = EnemyType.RoundCrazy;
				_roundCrazy.SpeedRange = new Vector2(10.0f,17.0f);
				_roundCrazy.CirclingRange = new Vector2(15,17);
				_roundCrazy.detectRange = 20.0f;
				_roundCrazy.health = 1.0f;
				_roundCrazy.SearchingRange = 0.0f;

				break;
			case EnemyType.TraceNormal:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Circle;
				_enemy.transform.localScale = Vector3.one;
				Enemy_Trace _trace = _enemy.AddComponent<Enemy_Trace>();
				_trace.enemyType = EnemyType.TraceNormal;
				_trace.SpeedRange = new Vector2(0.2f,0.5f);
				_trace.health = 1.0f;

				break;
			case EnemyType.TranceCrazy:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Circle;
				_enemy.GetComponent<TrailRenderer>().startWidth = 12;
				_enemy.transform.localScale = Vector3.one * 10;
				Enemy_Trace _traceCrazy = _enemy.AddComponent<Enemy_Trace>();
				_traceCrazy.enemyType = EnemyType.TranceCrazy;
				_traceCrazy.SpeedRange = new Vector2(0.5f,2.0f);
				_traceCrazy.health = 1.0f;

				break;
			case EnemyType.SpeedUp:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Circle;
				_enemy.transform.localScale = Vector3.one * 1;
				Enemy_SpeedUp _speedUp = _enemy.AddComponent<Enemy_SpeedUp>();
				_speedUp.enemyType = EnemyType.SpeedUp;
				_speedUp.SpeedRange = new Vector2(0.02f,0.05f);
				_speedUp.health = 1.0f;

				break;
			case EnemyType.SmartEnemy_Round:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Circle;
				_enemy.transform.localScale = Vector3.one * 1;
				SmartEnemy_SpeedUp _Smart_Round = _enemy.AddComponent<SmartEnemy_SpeedUp>();
				_Smart_Round.enemyType = EnemyType.SmartEnemy_Round;
				_Smart_Round.SpeedRange = new Vector2(0.2f,0.5f);
				_Smart_Round.health = 1.0f;

				break;
			case EnemyType.FearLight:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Arrow;
				_enemy.transform.localScale = Vector3.one * 0.5f;
				Enemy_FearLight _Fear_Light = _enemy.AddComponent<Enemy_FearLight>();
				_Fear_Light.enemyType = EnemyType.FearLight;
				_Fear_Light.SpeedRange = new Vector2(0.2f,0.5f);
				_Fear_Light.health = 1.0f;

				break;
			default:
				Debug.Log("Can't Create Anything, Sorry!!");
				return null;
		}
		_enemy.GetComponent<EnemyBase>().RegistHandler();
		_enemy.transform.SetParent(transform,false);
		//Complete making enemy

		enemyList.Add(_enemy);
		return _enemy;
	}

	public void createWaveAtPos(Event e)
	{
		CreateEnmey createEnemy = e as CreateEnmey;
		if(enemyList.Count<=MaxSpawnNum)
		{
			for(int i = 0; i<MaxSpawnNum; i++)
			{
				EnemyType types = (EnemyType)Random.Range(0,6);
				CreateEnemy(types,SetSpawnLocation(createEnemy._transform));
			}
		}
	}
	public void createEnemy_Amount_Around_Pos(EnemyType enemyType, Transform enemyTransform, int amount){
		for(int i = 0; i<amount; i++){
			enemyTransform.position = enemyTransform.position + new Vector3(Random.Range(-1.0f,1.0f),
																			Random.Range(-1.0f,1.0f),
																			0.0f);
			CreateEnemy(enemyType, enemyTransform);
		}
	}

	public void Kill(GameObject enemy)
	{
		Debug.Log(this.transform.parent.name);
		if(enemyList.Contains(enemy))
			enemyList.Remove(enemy);
		else
			Debug.Log("NOOOOOO");
		enemy.GetComponent<EnemyBase>().UnregistHandler();

		EnemyDestroy enemyDestroy = new EnemyDestroy();
		Service.eventManager.Fire(enemyDestroy);
		
		Destroy(enemy, 5.0f);
	}

	protected void CreateWave(Event e)
	{
		if(enemyList.Count <= MaxSpawnNum)
		{
			for(int i = 0; i<MaxSpawnNum; i++)
			{
				EnemyType types = (EnemyType)Random.Range(0,5);
				CreateEnemy(types, RandomSelectSpawnLocation());
			}
		}
	}
	public int CountAll()
	{
		return enemyList.Count;
	}

	public int CountType(EnemyType _enemyType)
	{
		return enemyList.FindAll(enemy => enemy.GetComponent<EnemyBase>().enemyType == _enemyType).Count;
	}

	public void KillThemAll()
	{
		for(int i = enemyList.Count - 1; i >= 0; i--){
			GameObject enemy = enemyList[i];
			enemyList[i].GetComponent<EnemyBase>().ApplyDamage(10);
		}
	}
	public void DestroyThemAll(){
		for(int i = enemyList.Count - 1; i >= 0; i--){
			GameObject enemy = enemyList[i];
			enemyList.Remove(enemy);

			enemy.GetComponent<EnemyBase>().UnregistHandler();
			Destroy(enemy);
		}
		foreach(GameObject enemy in enemyList){
			enemyList.Remove(enemy);
			enemy.GetComponent<EnemyBase>().UnregistHandler();

			Destroy(enemy);
		}
	}
}
