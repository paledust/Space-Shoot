using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
	// Use this for initialization
	public float speed;
	public GameObject bullet;
	public float bulletSpeed;
	public float ShootPause;

	private Vector3 velocity;
	private float shootTimer;

	void Start ()
	{
		velocity = Vector3.zero;
		shootTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Move ();
		Rotate ();

		if (Input.GetButton ("Fire1")) {
			shootTimer += Time.deltaTime;
			if (shootTimer >= ShootPause) {
				Shoot ();	
				shootTimer = 0.0f;
			}
		}

		if (Input.GetButtonUp ("Fire1")) {
			shootTimer = 0.0f;
		}
	}

	void Move ()
	{
		Vector3 idealVel = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), transform.position.z).normalized;
		velocity = Vector3.Lerp (velocity, idealVel, Time.deltaTime * speed);
		if (velocity.magnitude <= 0.01) {
			velocity = Vector3.zero;
		}
		transform.position += velocity;
	}

	void Rotate ()
	{
		float rotationDegree = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		if(velocity.magnitude != 0)
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationDegree);
	}

	void Shoot ()
	{
		GameObject new_Bullet = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
		new_Bullet.GetComponent<Rigidbody2D> ().velocity = transform.rotation * Vector2.right * bulletSpeed;
	}

	public Vector3 getVelocity()
	{
		return velocity;
	}
}
