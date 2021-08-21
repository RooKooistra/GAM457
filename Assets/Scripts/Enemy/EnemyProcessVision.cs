using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProcessVision : EnemyLogic
{
	public void ProcessVision(List<GameObject> visibleTargets, float viewRadius, float baseSuspicionAmount, float distanceMultiplyer, float VelocitySensitivity)
	{
		foreach (GameObject visibleTarget in visibleTargets)
		{
			int dbCount = 0;
			Transform thisLastKnownTransform = visibleTarget.transform; 
			Vector3 thisLastKnownVelocity = visibleTarget.GetComponent<CharacterController>().velocity;
			float visibleTargetDistance = Vector3.Distance(transform.position, visibleTarget.transform.position);

			float thisSuspicionScore = baseSuspicionAmount * DistanceMultiplyer(visibleTargetDistance, viewRadius, distanceMultiplyer) * VelocityMultiplyer(thisLastKnownVelocity.magnitude, VelocitySensitivity) * Time.deltaTime;

			foreach (Player player in players)
			{
				if (player.playerGameobject == visibleTarget)
				{
					player.suspicionScore += thisSuspicionScore;
					player.lastKnownTransform = thisLastKnownTransform;
					player.lastKnownVelocity = thisLastKnownVelocity;
					dbCount += 1;
				}
			}

			if (dbCount == 0)
			{
				Player newPlayer = new Player(visibleTarget, thisSuspicionScore, thisLastKnownTransform, thisLastKnownVelocity);
				players.Add(newPlayer);
			}
		}

	}

	float DistanceMultiplyer(float distance, float viewMaxDistance, float distanceSuspicionMultiplyer)
	{
		// Vision strength starts to fade after half distance. Max distance results in 0 -- ie: set your vision distance higher than desired
		return (viewMaxDistance - distance) / (viewMaxDistance - viewMaxDistance * 0.5f) * distanceSuspicionMultiplyer;
	}

	float VelocityMultiplyer(float playerVelocity, float velocitySensitivity)
	{
		// velocity magnitude currently not reading correctly
		return 1f;
	}
}
