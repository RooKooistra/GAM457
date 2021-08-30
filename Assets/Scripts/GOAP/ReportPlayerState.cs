using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class ReportPlayerState : RooAntAIState
{
	Movement movement;
	LogicModel logicModel;
	Hearing hearing;
	bool isPaused = true; // need to pause for a small time or i get a null reference error

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		movement = aGameObject.GetComponent<Movement>();
		logicModel =aGameObject.GetComponent<LogicModel>();
		hearing = aGameObject.GetComponent<Hearing>();
	}

	public override void Enter()
	{
		base.Enter();
		Invoke(nameof(UnlockPause), 0.5f); // was getting error running move agent immediately
	}

	void UnlockPause()
	{
		isPaused = false;
		hearing.soundLocation = Vector3.zero; // clear search light bug
	}

	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);
		if (isPaused) return;
		movement.MoveAgent(logicModel.GetClosestLastPlayerPosition());
	}
}
