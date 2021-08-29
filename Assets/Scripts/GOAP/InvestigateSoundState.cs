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

	public override void Enter()
	{
		base.Enter();
		//movement.agent.ResetPath();
	}

	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);
		if (!hearing.hasHearingLocation()) Finish();
		if (Vector3.Distance(transform.position, hearing.soundLocation) < 3f) // needed to be this number to stop state hanging if character got snagged
		{
			hearing.soundLocation = Vector3.zero;
			return;
		}
		movement.MoveAgent(hearing.soundLocation);
	}
}
