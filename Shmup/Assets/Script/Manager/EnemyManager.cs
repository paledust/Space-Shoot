using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType{
	RoundNormal = 0,
	RoundCrazy = 1,
	TraceNormal = 2,
	TranceCrazy = 3,
	SpeedUp = 4,
	Boss = 5
};

public class EnemyManager : MonoBehaviour {
	protected enum SpawnPattern{
		random = 0,
		circle = 1,
		oneByoneCircle = 2
	};
	public GameObject enemyBasicPrefeb;
	public List<GameObject> enemyList  = new List<GameObject>();
	public Transform[] SpawnLocation;
	public int MaxSpawnNum;
	public float waveCooldown;

	protected Sprite enemySprite_Circle;
	protected Sprite enemySprite_Arrow;

	public float EnemyRatio;
	private float timer;
	private bool ifWave;
	private int RoundNum;
	private int TraceNum;
	private SpawnPattern spawnPattern;
	// Use this for initialization
	void Start () {
		//EventManager.Instance.Register(play);
		//Initialize Enemy Player System
		if(!EnemyBase.enemyManager)
		{
			EnemyBase.enemyManager = this;
		}

		//Initialize Enemy Manager System
		if(!EnemyBase.player)
		{
			EnemyBase.player = GameObject.FindGameObjectWithTag("Player");
		}

		enemySprite_Circle = Resources.Load<Sprite>("Image/Enemy_1");
		enemySprite_Arrow = Resources.Load<Sprite>("Image/SpaceShip");

		ifWave = false;
		timer = 0.0f;

		EventManager.Instance.Register<EnemyWaveDestroy>(CreateWave);
		EventManager.Instance.Register<CreateEnmey>(CreateWave);
	}
	
	// Update is called once per frame
	void Update () {
		if(CountAll() == 0)
		{
			EnemyWaveDestroy waveDestroy = new EnemyWaveDestroy();
			EventManager.Instance.Fire(waveDestroy);
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			KillThemAll();
		}
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
		GameObject _enemy = Instantiate(enemyBasicPrefeb, enemyTransform) as GameObject;

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

				break;
			case EnemyType.RoundCrazy:
				_enemy.GetComponent<SpriteRenderer>().sprite = enemySprite_Arrow;
				Enemy_Round _roundCrazy = _enemy.AddComponent<Enemy_Round>();
				_roundCrazy.enemyType = EnemyType.RoundCrazy;
				_roundCrazy.SpeedRange = new Vector2(10.0f,17.0f);
				_roundCrazy.CirclingRange = new Vector2(15,17);
				_roundCrazy.detectRange = 20.0f;
				_roundCrazy.health = 1.0f;

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
			default:
				Debug.Log("Can't Create Anything, Sorry!!");
				return null;
		}
		_enemy.GetComponent<EnemyBase>().RegistHandler();
		//Complete making enemy

		enemyList.Add(_enemy);
		return _enemy;
	}

	public void createWaveAtPos(Event e)
	{
		CreateEnmey createEnemy = e as CreateEnmey;
		Debug.Log("Create Wave");
		for(int i = 0; i<MaxSpawnNum; i++)
		{
			EnemyType types = (EnemyType)Random.Range(0,5);
			CreateEnemy(types,SetSpawnLocation(createEnemy._transform));
		}
	}

	public void Destroy(GameObject enemy)
	{
		Debug.Log("Enemy:" + enemy.GetComponent<EnemyBase>().ToString() + "Destroy");
		enemyList.Remove(enemy);
		enemy.GetComponent<EnemyBase>().UnregistHandler();

		EnemyDestroy enemyDestroy = new EnemyDestroy();
		EventManager.Instance.Fire(enemyDestroy);
		
		Destroy(enemy, 5.0f);
	}

	protected void CreateWave(Event e)
	{
		Debug.Log("Create Wave");
		for(int i = 0; i<MaxSpawnNum; i++)
		{
			EnemyType types = (EnemyType)Random.Range(0,5);
			CreateEnemy(types, RandomSelectSpawnLocation());
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

	protected void KillThemAll()
	{
		foreach(GameObject enemy in enemyList)
		{
			enemy.GetComponent<EnemyBase>().ApplyDamage(10);
		}
	}
}
