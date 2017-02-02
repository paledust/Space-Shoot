using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
	static protected GameObject player;
	public Color32 ShipColor;
	public float health;
	public AudioClip destroySound;
	public float moveSpeed;
	protected Vector3 velocity;
	// Use this for initialization
	protected void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	protected void Start () {
		if(!GetComponent<AudioSource>())
			gameObject.AddComponent<AudioSource>();

		ColorInitial();
	}

	virtual protected void ColorInitial()
	{

	}
	
	// Update is called once per frame
	protected void Update () {
		Move();
		rotate();
		Kill();
	}

	virtual protected void Move()
	{
		
	}

	virtual protected void rotate()
	{

	}

	virtual public void ApplyDamage(float Damage)
	{
		health -= Damage;
	}

	protected void HitSound()
	{
		GetComponent<AudioSource>().PlayOneShot(destroySound);
	}
	protected void Kill()
	{
		HitSound();
		if(health <= 0.0f)
			Destroy(gameObject);
	}
}
