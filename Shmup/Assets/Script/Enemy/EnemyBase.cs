using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
	static protected GameObject player;
	static protected EnemyManager enemyManager;
	public Color32 ShipColor;
	public Vector2 SpeedRange;
	[SerializeField] protected float health;
	[SerializeField] protected AudioClip destroySound;
	[SerializeField] protected AudioClip spawnSound;
	[SerializeField] protected float moveSpeed;
	protected Vector3 velocity;
	protected bool ifMove = true;
	protected bool ifKill = false;
	// Use this for initialization
	protected void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
	}
	protected void Start () {
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();

		GetComponent<AudioSource>().PlayOneShot(spawnSound);
		moveSpeed = Random.Range(SpeedRange.x,SpeedRange.y);
		ColorInitial();
		MoveInitial();
	}

	//Initialize the Color 
	virtual protected void ColorInitial()
	{

	}

	//For movement Initialize
	virtual protected void MoveInitial()
	{
		
	}
	
	// Update is called once per frame
	protected void Update () {
		if(ifMove)
			Move();

		rotate();
		if(health <= 0.0f && !ifKill)
			Kill();
	}

	//Describe how the Enemy Move
	virtual protected void Move()
	{
		
	}

	//Describe how the Enemy Rotate
	virtual protected void rotate()
	{
		float rotationDegree = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		if(velocity.magnitude >= 0.01f)
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationDegree);
	}

	virtual public void ApplyDamage(float Damage)
	{
		health -= Damage;
		HitSound();
	}

	protected void HitSound()
	{
		GetComponent<AudioSource>().PlayOneShot(destroySound);
	}
	virtual protected void Kill()
	{
		ifKill = true;
		HitSound();
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		enemyManager.Destroy(gameObject);
		ifMove = false;
	}

	public void ResetTarget(GameObject target)
	{
		player = target;
	}
}
