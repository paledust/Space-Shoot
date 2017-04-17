using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {
	public float WaitTime = 2.0f;
	private float timer = 0.0f;
	// Use this for initialization
	void Update(){
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white,Color.yellow, timer/WaitTime);
	}
	void OnTriggerStay2D(Collider2D collider){
		if(collider.gameObject == Service.player){
			timer += Time.deltaTime;
			if(timer >= 1.0f){
				Enter_Level tempEvent = new Enter_Level();
				Service.eventManager.Fire(tempEvent);
				GetComponent<BoxCollider2D>().enabled = false;
			}
		}
	}
	void OnTriggerExit2D(Collider2D collider){
		if(collider.gameObject == Service.player){
			timer = 0.0f;
		}
	}
	public void Reset(){
		timer = 0.0f;
	}
}
