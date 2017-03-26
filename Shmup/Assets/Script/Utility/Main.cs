using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
	void Awake()
	{
		Service.player = GameObject.FindGameObjectWithTag("Player");
	}
}
