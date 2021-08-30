using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class CollectEnergyState : AntAIState
{
	Health health;
	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		health = aGameObject.GetComponent<Health>();
	} 

	public override void Enter()
	{
		base.Enter();
		health.healthLevel = health.maxHealth;
	}
}
