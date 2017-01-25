﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {
	public Transform lockTransform;
	public float Follow_Speed;

	private bool ifFollow;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(lockTransform.position.x, lockTransform.position.y, transform.position.z);

		ifFollow = true;
	}
	
	// Update is called once per frame
	void Update () {
		CameraMove();
	}

	void CameraMove()
	{
		if(ifFollow)
			transform.position = Vector3.Slerp(transform.position, 
												new Vector3(lockTransform.position.x, lockTransform.position.y, transform.position.z),
												Time.deltaTime * Follow_Speed);
	}
}
