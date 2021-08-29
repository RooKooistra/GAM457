using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class CollectEnergyState : AntAIState
{
	Health health;
	Hearing hearing;
	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		health = aGameObject.GetComponent<Health>();
		hearing = aGameObject.GetComponent<Hearing>(); 
	} 

	public override void Enter()
	{
		base.Enter();
		health.healthLevel = health.maxHealth;
		hearing.soundLocation = Vector3.zero; // only have this here to clear a soundlocation not being cleared error
	}
}
