using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BT_Hacks : MonoBehaviour
{
    // stuff to make BT work

    [HideInInspector]
    public Vector3 BTDestination;
    [HideInInspector]
    public Vector3 BTLastKnownDestination;

    LogicModel logicModel;
    NavMeshAgent agent;
    Movement movement;
    Health health;
    public List<Vector3> energyPositions = new List<Vector3>();
    void Start()
    {
        GameObject[] energyObjects = GameObject.FindGameObjectsWithTag("Energy");

        foreach (GameObject GO in energyObjects)
		{
            energyPositions.Add(GO.transform.position);
		}

        health = GetComponent<Health>();
        logicModel = GetComponent<LogicModel>();
        movement = GetComponent<Movement>();
        agent = GetComponent<NavMeshAgent>();
        LogicModel.FireGuns += HandleFireGuns;
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Energy") health.healthLevel = health.maxHealth;
	}
	private void OnDestroy()
    {
        LogicModel.FireGuns -= HandleFireGuns;
    }

    void HandleFireGuns(GameObject playerGO)
	{
        BTDestination = playerGO.transform.position;
	}

    public bool AgentHasPath()
	{
        return (agent.remainingDistance < 2f) ? true : false;
	}

    public void GetEnergy()
	{
        movement.MoveAgent(logicModel.GetClosestPosition(energyPositions));
	}
}
