using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Action();
public class ActionTask : Task {
	private readonly Action action;
	public ActionTask(Action m_action)
	{
		action = m_action;
	}
	override protected void Init()
	{
		SetStatus(TaskStatus.Success);
		action();
	}
}
