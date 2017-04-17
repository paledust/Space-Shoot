using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
	static public EnemyManager enemyManager;
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
		taskManager = new Task_Manager();
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();

		moveSpeed = Random.Range(SpeedRange.x,SpeedRange.y);
		GetComponent<AudioSource>().PlayOneShot(spawnSound);
		ColorInitial();
		MoveInitial();
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
		taskManager.Update();
		rotate();

		if(health <= 0.0f && !ifKill)
			Kill();
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
	virtual protected void Kill()
	{
		Service.enemyManager.Kill(gameObject);
		ifKill = true;
		HitSound();
		this.enabled = false;
		GetComponent<SpriteRenderer>().color = Color.gray;
		if(GetComponent<TrailRenderer>())
		{
			GetComponent<TrailRenderer>().startColor = Color.gray;
			GetComponent<TrailRenderer>().endColor = Color.gray;
		}
		
		GetComponent<Collider2D>().enabled = false;
	}

	//Apply the damage to player
	public void ApplyDamage(float Damage)
	{
		health -= Damage;
		HitSound();
	}
	virtual protected void MoveInitial(){}
	virtual public void RegistHandler(){}
	virtual public void UnregistHandler(){}
	//Surround the Player, it's a tool function
	protected void Circling(float circlingRadius){
		Vector3 toPlayer = Service.player.transform.position - transform.position;
		velocity = (toPlayer + Quaternion.Euler(0,0,90) * toPlayer.normalized * circlingRadius).normalized * moveSpeed;
	}

	//Set the Velocity Toward Player, it's a tool function
	protected void TowardPlayer(float Agility){
		velocity = Vector3.Lerp(velocity, (Vector3)((Vector2)Service.player.transform.position - (Vector2)transform.position).normalized * moveSpeed, Agility * Time.deltaTime);
	}
	protected void FleePlayer(float Agility){
		velocity = Vector3.Lerp(velocity, (Vector3)((Vector2)transform.position - (Vector2)Service.player.transform.position).normalized * moveSpeed, Agility * Time.deltaTime);
	}
	protected void CopyMovement(Quaternion originalRotation){
		Vector3 playerVelocity = Service.player.GetComponent<Control>().getVelocity();
		
		velocity = (originalRotation * playerVelocity.normalized).normalized * playerVelocity.magnitude * moveSpeed;
	}
	protected void FleeFromTarget(float Agility){
		Debug.Log("Flee");
		velocity = Vector3.Lerp(velocity, new Vector3(Random.Range(-10.0f,10.0f), Random.Range(-10.0f,10.0f), 0.0f), Agility * Time.deltaTime);
	}

	//Reset the target into something else, it's a tool function
	protected void ResetTarget(GameObject target){
		Service.player = target;
	}

	protected void ShootBullet()
	{
		Quaternion lookVec = Quaternion.Euler(0,0,Random.Range(0,360));
		GameObject new_Bullet = Instantiate (bulletPrefeb, transform.position + lookVec * Vector2.right * 20, lookVec) as GameObject;
		new_Bullet.GetComponent<Rigidbody2D> ().velocity = lookVec * Vector2.right * 10.0f;
	}
	protected Vector3 DISTANCE_TO_PLAYER()
	{
		return transform.position - Service.player.transform.position;
	}
}
