using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class ReportPlayerState : AntAIState
{
	Movement movement;
	LogicModel logicModel;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		movement = aGameObject.GetComponent<Movement>();
		logicModel = GetComponent<LogicModel>();
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);
		movement.MoveAgent(logicModel.GetClosestLastPlayerPosition());
	}
}
