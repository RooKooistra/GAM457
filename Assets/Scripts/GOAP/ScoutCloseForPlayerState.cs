using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;

public class ScoutCloseForPlayerState : AntAIState
{
	Vision vision;
	Movement movement;
	Hearing hearing;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		movement = aGameObject.GetComponent<Movement>();
		vision = aGameObject.GetComponent<Vision>();
		hearing = aGameObject.GetComponent<Hearing>();
	}

	public override void Enter()
	{
		base.Enter();
		movement.agent.ResetPath();
		hearing.soundLocation = Vector3.zero; // clearing search light bug
		StartCoroutine(ScoutForPlayer(vision.GetSearchArea()));	
	}

	IEnumerator ScoutForPlayer(List<Vector3> path)
	{
		int activeIndex = 0;
		while (true)
		{
			if (Vector3.Distance(transform.position, path[activeIndex]) < 1f || movement.agent.hasPath == false)
			{
				movement.agent.destination = path[activeIndex];
				activeIndex++;
				if (activeIndex >= path.Count)
				{
					Finish(); // node will run again with the same remembered last player position if memory cooldown has not finished
					yield break;
				}
			}

			yield return null;
		}
	}

}
