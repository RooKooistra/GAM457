using Anthill.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForEnergyState : AntAIState
{
	Movement movement;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		movement = aGameObject.GetComponent<Movement>();
	}

	public override void Enter()
	{
		base.Enter();
		movement.FindEnergy();
	}

	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);
		if (movement.agent.remainingDistance < 1f) Finish();
	}

}


