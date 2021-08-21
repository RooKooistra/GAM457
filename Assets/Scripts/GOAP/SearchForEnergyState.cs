using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Anthill.AI;
using Anthill.Utils;

public class SearchForEnergyState : AntAIState
{
    NavMeshAgent agent;
    
    // testing to see if this works
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        agent.destination = Vector3.zero;
    }

}
