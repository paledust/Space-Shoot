using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPattern{
	random = 0,
	circle = 1,
	oneByoneCircle = 2
}

public class EnemyManager : MonoBehaviour {
	public GameObject[] EnemyTypes;

	public List<GameObject> enemyrounds = new List<GameObject>();
	public List<GameObject> enemyTrace = new List<GameObject>();
	public Transform[] SpawnLocation;
	public int MaxSpawnNum;
	public float waveCooldown;

	public float EnemyRatio;
	private float timer;
	private bool ifWave;
	private int RoundNum;
	private int TraceNum;
	private SpawnPattern spawnPattern;
	// Use this for initialization
	void Start () {
		GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject enemy in allEnemy)
		{
			if(enemy.GetComponent<Enemy_Round>())
			{
				enemyrounds.Add(enemy);
			}
			if(enemy.GetComponent<Enemy_Trace>())
			{
				enemyTrace.Add(enemy);
			}
		}

		ifWave = false;
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!ifWave && CountAll() == 0)
		{
			Debug.Log("Waiting");
			timer += Time.deltaTime;
			if(timer >= waveCooldown)
				ifWave = true;
		}

		if(ifWave)
		{
			for(int i = 0; i<MaxSpawnNum; i++)
			{
				Create(EnemyTypes[Random.Range(0,EnemyTypes.Length)]);
			}

			ifWave = false;
		}
	}

	public void Create(GameObject enemy)
	{
		Debug.Log("Enemy:" + enemy.GetComponent<EnemyBase>().ToString() + "Create");
		Instantiate(enemy, SpawnLocation[Random.Range(0,SpawnLocation.Length)].position,
							SpawnLocation[Random.Range(0,SpawnLocation.Length)].rotation);

		if(enemy.GetComponent<Enemy_Round>())
		{
			enemyrounds.Add(enemy);
		}
		if(enemy.GetComponent<Enemy_Trace>())
		{
			enemyTrace.Add(enemy);
		}

		OnCreated();
	}

	public void Destroy(GameObject enemy)
	{
		Debug.Log("Enemy:" + enemy.GetComponent<EnemyBase>().ToString() + "Destroy");

		if(enemy.GetComponent<Enemy_Round>())
			enemyrounds.Remove(enemy);
		if(enemy.GetComponent<Enemy_Trace>())
			enemyTrace.Remove(enemy);

		Destroy(enemy, 5.0f);

		OnDestroyed();
	}

	public void FactoryCreate(GameObject enemy)
	{

	}

	public int CountAll()
	{
		return enemyrounds.Count + enemyTrace.Count;
	}

	public int CountRound()
	{
		return enemyrounds.Count;
	}
	
	public int CountTrace()
	{
		return enemyTrace.Count;
	}

	//Update Information inside the Manager when create
    private void OnCreated()
    {
        Debug.Log("Created " + " Enemy");
    }

	//Update Information inside the Manager when destroy
    private void OnDestroyed()
    {
        Debug.Log("Destroyed " + " Enemy");
    }
}
