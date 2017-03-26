using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
	static public GameObject player;
	static public EnemyManager enemyManager;
	public FSM<EnemyBase> fsm{get;set;}
	public Task_Manager taskManager{get;set;}
	public GameObject bulletPrefeb;
	public Vector2 SpeedRange;
	public float health;
	public EnemyType enemyType;
	[SerializeField] protected AudioClip destroySound;
	[SerializeField] protected AudioClip spawnSound;
	protected float moveSpeed;
	protected Color32 ShipColor;
	protected Vector3 velocity;
	protected bool ifKill = false;
	protected virtual void Start () {
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();

		moveSpeed = Random.Range(SpeedRange.x,SpeedRange.y);
		GetComponent<AudioSource>().PlayOneShot(spawnSound);
		ColorInitial();
		MoveInitial();
		fsm = new FSM<EnemyBase>(this);
		fsm.TransitionTo<Searching>();
		taskManager = new Task_Manager();
	}

	//Initialize the Color 
	protected void ColorInitial()
	{
		ShipColor = Service.ColorLibrary[(int)Random.Range(0,Service.ColorLibrary.Length)];

		GetComponent<SpriteRenderer>().color = ShipColor;

		if(GetComponent<TrailRenderer>())
		{
			GetComponent<TrailRenderer>().startColor = ShipColor;
			GetComponent<TrailRenderer>().endColor = ShipColor;
		}
	}

	// Update is called once per frame
	protected virtual void Update () {
		fsm.Update();
		taskManager.Update();
		rotate();

		if(health <= 0.0f && !ifKill)
			Kill();
		
	}

	//Describe how the Enemy Rotate
	protected void rotate()
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
		this.enabled = false;
		GetComponent<SpriteRenderer>().color = Color.gray;
		if(GetComponent<TrailRenderer>())
			GetComponent<TrailRenderer>().startColor = Color.gray;

		GetComponent<TrailRenderer>().endColor = Color.gray;
		GetComponent<Collider2D>().enabled = false;
		enemyManager.Destroy(gameObject);
	}

	//Apply the damage to player
	public void ApplyDamage(float Damage)
	{
		health -= Damage;
		HitSound();
	}
	virtual internal void OtherMove()
	{}
	virtual protected void MoveInitial()
	{}
	virtual protected void SearchingMove()
	{}
	virtual protected void PreSurroundingMove()
	{
		TowardPlayer(2.0f);
		transform.position -= velocity;
	}
	virtual protected void SurroundingMove()
	{
		Circling(150.0f);
		
		velocity *= 0.99f;
		transform.position += velocity;
	}
	virtual protected void FleeingMove()
	{
		TowardPlayer(0.8f);
		transform.position -= velocity;
	}
	virtual public void RegistHandler()
	{}
	virtual public void UnregistHandler()
	{}

	//Surround the Player, it's a tool function
	protected void Circling(float circlingRadius)
	{
		Vector3 toPlayer = player.transform.position - transform.position;
		velocity = (toPlayer + Quaternion.Euler(0,0,90) * toPlayer.normalized * circlingRadius).normalized * moveSpeed;
	}

	//Set the Velocity Toward Player, it's a tool function
	protected void TowardPlayer(float Agility)
	{
		velocity = Vector3.Lerp(velocity, (Vector3)((Vector2)player.transform.position - (Vector2)transform.position).normalized * moveSpeed, Agility * Time.deltaTime);
	}
	protected void Patrol()
	{
		
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

	protected void ShootBullet()
	{
		Quaternion lookVec = Quaternion.Euler(0,0,Random.Range(0,360));
		GameObject new_Bullet = Instantiate (bulletPrefeb, transform.position + lookVec * Vector2.right * 20, lookVec) as GameObject;
		new_Bullet.GetComponent<Rigidbody2D> ().velocity = lookVec * Vector2.right * 10.0f;
	}
	void OnDestroy()
	{
		fsm.Clear();
	}


	public class EnemyState : FSM<EnemyBase>.State {
		protected Vector3 getDistanceToPlayer()
		{
			return Context.gameObject.transform.position - Service.player.transform.position;
		}
	}

	public class Searching: EnemyState{
		public override void Update()
		{
			if(getDistanceToPlayer().magnitude <= 8.0f) 
				TransitionTo<Surr_Prep>();
			Context.SearchingMove();
		}
	}
	public class Surr_Prep: EnemyState{
		public override void OnEnter()
		{
			Context.PreSurroundingMove();
			TransitionTo<Surr>();
		}
		public override void Update()
		{
			TransitionTo<Surr>();
		}
	}
	public class Surr: EnemyState{
		public override void Update()
		{
			Context.SurroundingMove();
			if(getDistanceToPlayer().magnitude >= 10.0f)
				TransitionTo<Searching>();
		}
	}
	public class Flee: EnemyState{
		public override void Update()
		{
			Context.FleeingMove();
		}
	}
}
