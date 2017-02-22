using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
	static public GameObject player;
	static public EnemyManager enemyManager;
	public Vector2 SpeedRange;
	public float health;
	public EnemyType enemyType;
	[SerializeField] protected AudioClip destroySound;
	[SerializeField] protected AudioClip spawnSound;
	[SerializeField] protected float moveSpeed;
	protected Color32 ShipColor;
	protected Vector3 velocity;
	protected bool ifMove = true;
	protected bool ifKill = false;
	// Use this for initialization
	protected void Awake()
	{
		//player = GameObject.FindGameObjectWithTag("Player");
		//enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
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
	protected void ColorInitial()
	{
		ShipColor = ColorChoose.ColorLibrary[(int)Random.Range(0,ColorChoose.ColorLibrary.Length)];

		GetComponent<SpriteRenderer>().color = ShipColor;

		if(GetComponent<TrailRenderer>())
		{
			GetComponent<TrailRenderer>().startColor = ShipColor;
			GetComponent<TrailRenderer>().endColor = ShipColor;
		}
	}

	//For movement Initialize
	protected void MoveInitial()
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

	//Describe how the Enemy Rotate
	private void rotate()
	{
		float rotationDegree = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		if(velocity.magnitude >= 0.01f)
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationDegree);
	}

	//Play Hit Sound
	protected void HitSound()
	{
		GetComponent<AudioSource>().PlayOneShot(destroySound);
	}

	//Called when be killed
	protected void Kill()
	{
		ifKill = true;
		HitSound();
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		enemyManager.Destroy(gameObject);
		ifMove = false;
	}

	//Apply the damage to player
	public void ApplyDamage(float Damage)
	{
		health -= Damage;
		HitSound();
	}

	//Describe how the Enemy Move, it's a sandbox function.
	virtual protected void Move()
	{
		
	}

	//Suppose to be called outside the 
	virtual public void RegistHandler()
	{

	}

	virtual public void UnregistHandler()
	{

	}
	//Surround the Player, it's a tool function
	protected void Circling(float circlingRadius)
	{
		Vector3 toPlayer = player.transform.position - transform.position;
		velocity = (toPlayer + Quaternion.Euler(0,0,90) * toPlayer.normalized * circlingRadius).normalized * moveSpeed;
	}

	//Set the Velocity Toward Player, it's a tool function
	protected void TowardPlayer(float Agility)
	{
		velocity = Vector3.Lerp(velocity, player.transform.position - transform.position, Agility * Time.deltaTime);
	}

	protected void CopyMovement(Quaternion originalRotation)
	{
		Vector3 playerVelocity = player.GetComponent<Control>().getVelocity();
		
		velocity = (originalRotation * playerVelocity.normalized).normalized * playerVelocity.magnitude * moveSpeed;
	}

	//Reset the target into something else, it's a tool function
	protected void ResetTarget(GameObject target)
	{
		player = target;
	}
}
