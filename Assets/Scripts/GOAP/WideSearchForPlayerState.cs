using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;
using UnityEngine.AI;

public class WideSearchForPlayerState : AntAIState
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
		movement.FindPlayer();
	}
}
