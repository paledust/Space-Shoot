using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTask : Task {
	private float timer = 0.0f;
	private readonly float duration;
	public TimeTask(float _duration)
	{
		duration = _duration;
	}
	override protected void Init()
	{
		timer = 0.0f;
	}
	override internal void TUpdate()
	{
		timer += Time.deltaTime;
		if(timer >= duration)
		{
			CountOver();
		}
		else
		{
			OnTick(timer);
		}
	}
	private void OnTick(float t)
	{}
	private void CountOver()
	{}

}
