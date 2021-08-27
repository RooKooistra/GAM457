using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;
using Anthill.Utils;

public class EnemySense : MonoBehaviour, ISense
{
	LogicModel logicModel;
	Vision vision;
	Health health;

	private void Awake()
	{
		logicModel = GetComponent<LogicModel>();
		vision = GetComponent<Vision>();
		health = GetComponent<Health>();
		
	}
	public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
	{
		vision.FindVisibleTargets();
		logicModel.MonitorPlayers();

		// set worldstate variables
		aWorldState.Set(EnemyScoutVariables.LowEnergy, health.isLowHealth());
		aWorldState.Set(EnemyScoutVariables.CanSeeEnergy, logicModel.canSeeEnergy);
		aWorldState.Set(EnemyScoutVariables.CanSeePlayer, logicModel.canSeePlayer);
		aWorldState.Set(EnemyScoutVariables.KnowPlayersLastPosition, logicModel.remembersPlayerLocation());
		aWorldState.Set(EnemyScoutVariables.KnowEnergyLocation, logicModel.knowEnergyLocation());
		aWorldState.Set(EnemyScoutVariables.PlayerAlive, true);
		aWorldState.Set(EnemyScoutVariables.IsJoinedToHost, false);
		aWorldState.Set(EnemyScoutVariables.IsAtEnergyLocation, logicModel.checkNextToEnergy());
	}

}
