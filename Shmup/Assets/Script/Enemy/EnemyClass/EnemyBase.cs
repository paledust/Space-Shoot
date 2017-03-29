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
	public float SearchingRange = 20.0f;
	public float FleeRange = 50.0f;
	protected float moveSpeed;
	protected Color32 ShipColor;
	protected Vector3 velocity;
	protected bool ifChangingColor = false;
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
		EventManager.Instance.Register<BossDie>(BossDieHandler);
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

		if(ifChangingColor)
			ChangingColor();

		
	}
	protected void ChangingColor()
	{
		Color tempColor = Service.ColorLibrary[Random.Range(0,4)];
		GetComponent<SpriteRenderer>().color = tempColor;

		if(GetComponent<TrailRenderer>())
		{
			GetComponent<TrailRenderer>().startColor = tempColor;
			GetComponent<TrailRenderer>().endColor = tempColor;
		}
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
		ifChangingColor = false;
		HitSound();
		this.enabled = false;
		GetComponent<SpriteRenderer>().color = Color.gray;
		if(GetComponent<TrailRenderer>())
		{
			GetComponent<TrailRenderer>().startColor = Color.gray;
			GetComponent<TrailRenderer>().endColor = Color.gray;
		}
		
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
		FleePlayer(2.0f);

		transform.position += velocity;
	}
	virtual protected void SurroundingMove()
	{
		Circling(150.0f);
		
		velocity *= 0.99f;
		transform.position += velocity;
	}
	virtual protected void FleeingMove()
	{
		FleePlayer(2.0f);

		transform.position += velocity;
	}
	virtual public void RegistHandler()
	{}
	virtual public void UnregistHandler()
	{}

	protected void BossDieHandler(Event e)
	{
		fsm.TransitionTo<Madness>();
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
		velocity = Vector3.Lerp(velocity, (Vector3)((Vector2)player.transform.position - (Vector2)transform.position).normalized * moveSpeed, Agility * Time.deltaTime);
	}
	protected void FleePlayer(float Agility)
	{
		velocity = Vector3.Lerp(velocity, (Vector3)((Vector2)transform.position - (Vector2)player.transform.position).normalized * moveSpeed, Agility * Time.deltaTime);
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
		private float timer = 0.0f;
		private Vector3 originalScale;
		public override void Init()
		{
			timer = 0.0f;
			Context.ifChangingColor = false;
			originalScale = Context.transform.localScale;
		}
		public override void OnEnter()
		{
			timer = 0.0f;
			Context.ifChangingColor = false;
			Debug.Log("Transfer");
		}
		public override void Update()
		{
			if(Context.transform.localScale != originalScale)
				Context.transform.localScale = Vector3.Lerp(originalScale * 5, originalScale, Easing.BackEaseInOut(timer));
			timer += Time.deltaTime;
			if(getDistanceToPlayer().magnitude <= 10.0f) 
				TransitionTo<Surr_Prep>();
			Context.SearchingMove();
		}
	}
	public class Surr_Prep: EnemyState{
		private float timer;
		private Vector3 originalScale;
		public override void Init()
		{
			Context.ifChangingColor = true;
			originalScale = Context.transform.localScale;
			timer = 0.0f;

			EventManager.Instance.Register<EnemyDestroy>(escape);
		}
		public override void OnEnter()
		{
			Context.ifChangingColor = true;
			timer = 0.0f;
		}
		public override void Update()
		{
			timer += Time.deltaTime;
			Context.transform.localScale = Vector3.Lerp(originalScale, originalScale * 5, Easing.BackEaseInOut(timer));

			Context.PreSurroundingMove();
			if(Context.transform.localScale == originalScale * 5)
				TransitionTo<Surr>();
		}
		private void escape(Event e)
		{
			TransitionTo<Flee>();
		}
	}
	public class Surr: EnemyState{
		public override void Update()
		{
			Context.SurroundingMove();
			if(getDistanceToPlayer().magnitude >= Context.SearchingRange)
				TransitionTo<Searching>();
		}
	}
	public class Flee: EnemyState{
		public override void Update()
		{
			if(getDistanceToPlayer().magnitude >= Context.FleeRange)
				TransitionTo<Searching>();

			Context.FleeingMove();
		}
	}
	public class Madness: EnemyState{
		float circlingRadius;
		float speed;
		float timer;
		public override void Init()
		{
			timer = 0.0f;
			circlingRadius = 150;
			speed = 2.0f;

			Context.moveSpeed = speed;
		}
		public override void Update()
		{
			timer += Time.deltaTime;

			Context.moveSpeed = Mathf.Lerp(speed, 20.0f, Easing.BackEaseIn(timer/7.0f));
			circlingRadius = Mathf.Lerp(150.0f, 3.0f, Easing.BackEaseIn(timer/7.0f));

			Context.Circling(circlingRadius);

			Context.transform.position += Context.velocity;
		}
	}
}
