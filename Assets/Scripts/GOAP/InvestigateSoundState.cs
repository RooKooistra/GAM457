using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class InvestigateSoundState : AntAIState
{
	Movement movement;
	Hearing hearing;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		movement = aGameObject.GetComponent<Movement>();
		hearing = aGameObject.GetComponent<Hearing>();
	}


	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);
		if (!hearing.hasHearingLocation()) Finish();
		if (movement.agent.remainingDistance < 1f)
		{
			hearing.soundLocation = Vector3.zero;
			return;
		}
		movement.MoveAgent(hearing.soundLocation);
	}
}
