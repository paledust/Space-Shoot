using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomGenerate : MonoBehaviour {
	public GameObject[] enemyPrefeb;
	public float coolDown = 10.0f;
	public int EnemyNumberRange;

	private float timer;
	// Use this for initialization
	void Start () {
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if(timer >= coolDown)
		{
			int num = Random.Range(0,EnemyNumberRange);
			for(int i = 0;i<num; i++)
			{
				Instantiate(enemyPrefeb[Random.Range(0,enemyPrefeb.Length)],transform.position,transform.rotation);
			}
			timer = 0.0f;
		}
	}
}
