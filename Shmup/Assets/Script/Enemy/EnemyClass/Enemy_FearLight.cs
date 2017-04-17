using UnityEngine;

public class Enemy_FearLight : EnemyBase {
	private bool ifActivated = false;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		GetComponent<SpriteRenderer>().color = Color.white;

		if(GetComponent<TrailRenderer>())
		{
			GetComponent<TrailRenderer>().startColor = Color.white;
			GetComponent<TrailRenderer>().endColor = Color.white;
		}
		Service.eventManager.Register<Enter_Level>(Flee);
	}
	protected override void Update(){
		base.Update();
		if(ifActivated){
			FleePlayer(1.0f);		
			transform.position += velocity;
		}
	}
	void Flee(Event e){
		ifActivated = true;
	}

}
