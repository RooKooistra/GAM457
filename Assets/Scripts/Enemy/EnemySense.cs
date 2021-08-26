using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;
using Anthill.Utils;

public class EnemySense : MonoBehaviour, ISense
{
	LogicModel logicModel = null;
	Vision vision = null;

	private void Awake()
	{
		logicModel = GetComponent<LogicModel>();
		vision = GetComponent<Vision>();
	}
	public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
	{
		vision.FindVisibleTargets();
		logicModel.MonitorPlayers();

		// set worldstate variables
		aWorldState.Set(EnemyScoutVariables.LowEnergy, false);
		aWorldState.Set(EnemyScoutVariables.CanSeeEnergy, false);
		aWorldState.Set(EnemyScoutVariables.CanSeePlayer, false);
		aWorldState.Set(EnemyScoutVariables.KnowPlayersLastPosition, false);
		aWorldState.Set(EnemyScoutVariables.KnowEnergyLocation, false);
		aWorldState.Set(EnemyScoutVariables.PlayerAlive, true);
		aWorldState.Set(EnemyScoutVariables.IsJoinedToHost, false);
		aWorldState.Set(EnemyScoutVariables.IsAtEnergyLocation, false);
	}
}
