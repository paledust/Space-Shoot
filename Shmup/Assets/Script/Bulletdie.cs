using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletdie : MonoBehaviour {
	public float lifeTime;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,lifeTime);
	}
}
