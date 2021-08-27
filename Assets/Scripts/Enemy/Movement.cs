using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent;
    LogicModel logicModel;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        logicModel = GetComponent<LogicModel>();
    }


    public void MoveAgent(Vector3 target)
	{
        agent.destination = target;
	}

    public void FindEnergy()
	{
        Vector3 posToMove = (logicModel.energyLocations.Count > 0) ? logicModel.GetClosestItem(logicModel.energyLocations) : RandomPosition();
        MoveAgent(posToMove);
	}

    public void FindPlayer()
	{
        MoveAgent(RandomPosition());
	}

    /// <summary>
    /// will switch to an vector2 with draw gizmos
    /// </summary>
    /// <returns></returns>
    Vector3 RandomPosition()
	{
        return new Vector3(
            Random.Range(-40, 40),
            0,
            Random.Range(-40, 40));
	}
}
