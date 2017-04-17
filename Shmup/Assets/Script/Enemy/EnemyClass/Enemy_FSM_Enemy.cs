using UnityEngine;

public class Enemy_FSM_Enemy : EnemyBase {
	public float SearchingRange = 20.0f;
	public float FleeRange = 50.0f;
	public FSM<Enemy_FSM_Enemy> fsm{get;set;}
	protected bool ifChangingColor = false;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		fsm = new FSM<Enemy_FSM_Enemy>(this);
		fsm.TransitionTo<Searching>();
		Service.eventManager.Register<BossDie>(BossDieHandler);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		fsm.Update();

		if(ifChangingColor)
			ChangingColor();
	}
	virtual protected void SearchingMove(){}
	virtual protected void PreSurroundingMove(){
		FleePlayer(2.0f);
		transform.position += velocity;
	}
	virtual protected void SurroundingMove(){
		Circling(150.0f);
		
		velocity *= 0.99f;
		transform.position += velocity;
	}
	virtual protected void FleeingMove(){
		FleePlayer(2.0f);
		transform.position += velocity;
	}
	virtual internal void OtherMove(){}
	protected void BossDieHandler(Event e){
		fsm.TransitionTo<Madness>();
	}
	void OnDestroy(){
		fsm.Clear();
	}
	protected override void Kill()
	{
		base.Kill();
		ifChangingColor = false;
	}

	//State Mechine Class
	public class EnemyState : FSM<Enemy_FSM_Enemy>.State {
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
		}
		public override void Update()
		{
			if(Context.transform.localScale != originalScale)
				Context.transform.localScale = Vector3.Lerp(originalScale * 5, originalScale, Easing.BackEaseInOut(timer));
			timer += Time.deltaTime;
			if(getDistanceToPlayer().magnitude <= Context.SearchingRange) 
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

			Service.eventManager.Register<EnemyDestroy>(escape);
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
