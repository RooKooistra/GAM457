using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class SetStates : MonoBehaviour
{
	AntAIAgent antAIAgent;

	private void Start()
	{
		antAIAgent = GetComponent<AntAIAgent>();

		antAIAgent.worldState.BeginUpdate(antAIAgent.planner);

		antAIAgent.worldState.Set(0, false);
		antAIAgent.worldState.Set(1, false);
		antAIAgent.worldState.Set(2, false);
		antAIAgent.worldState.Set(3, false);
		antAIAgent.worldState.Set(4, false);
		antAIAgent.worldState.Set(5, true);
		antAIAgent.worldState.Set(6, false);
		antAIAgent.worldState.Set(7, false);
	}
}
