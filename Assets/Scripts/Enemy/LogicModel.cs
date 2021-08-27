using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogicModel : MonoBehaviour
{
    List<RememberedPlayer> rememberedPlayersData = new List<RememberedPlayer>();
	public List<Vector3> energyLocations = new List<Vector3>();
	public List<Vector3> POILocations = new List<Vector3>();

	public Text dbText = null; // for better live debugging

	[Range(0, 20)] public float cooldownRate = 10f;

	public float monitorTick = 0.3f;

	public bool canSeePlayer = false;

	public static event Action Alerted;
	public static event Action Calm;

	private void Start()
	{
		StartCoroutine(SetCalmDelayed(0.1f));
	}

	/// <summary>
	/// Calm() could not be called at start..
	/// </summary>
	/// <param name="delaytime"></param>
	/// <returns></returns>
	IEnumerator SetCalmDelayed(float delaytime)
	{
		yield return new WaitForSeconds(delaytime);
		Calm();
	}

	/// <summary>
	/// Monitors player list to see if any are visible
	/// </summary>
	/// <param name="delay"></param>
	/// <returns></returns>
	public void MonitorPlayers()
	{
			canSeePlayer = false;
			if (rememberedPlayersData.Count > 0)
			{
				float totalSuspicionCount = 0;

				foreach (RememberedPlayer playerData in rememberedPlayersData)
				{
					float suspicion = (playerData.suspicionScore - cooldownRate * Time.deltaTime > 0f) ? (playerData.suspicionScore - cooldownRate * Time.deltaTime > 1f)? 1: playerData.suspicionScore - cooldownRate * Time.deltaTime : 0f;
					playerData.suspicionScore = suspicion;

					if (suspicion >= 1f)
					{
						canSeePlayer = true;
						Alerted();
					}
					totalSuspicionCount += suspicion;

					string sussText = (suspicion > 0) ? (suspicion > 1) ? "SEEN" : Mathf.Round(suspicion * 100).ToString()+"%" : "None";
				    if(dbText!=null) dbText.text = sussText;
				}

				if (totalSuspicionCount == 0) Calm();
			}
	}

	/// <summary>
	/// Processes the list of things that are currently in view of the vision system. puts them in a known list and if a player calculates suspicion (0-1)
	/// when suspicion reaches 1 = player can be seen
	/// </summary>
	/// <param name="visibleTargets"></param>
	/// <param name="viewRadius"></param>
	/// <param name="baseSuspicionAmount"></param>
	/// <param name="distanceMultiplyer"></param>
	/// <param name="VelocitySensitivity"></param>

	public void ProcessVision(List<GameObject> visibleTargets, float viewRadius, float baseSuspicionAmount, float distanceMultiplyer, float VelocitySensitivity)
	{
		foreach (GameObject visibleTarget in visibleTargets)
		{
			if (visibleTarget.GetComponent<CharacterController>()) // if the target is a player
			{
				int dbCount = 0;
				Transform thisLastKnownTransform = visibleTarget.transform;
				Vector3 thisLastKnownVelocity = visibleTarget.GetComponent<CharacterController>().velocity; // used this instead of rigid body as I couldnt get a velocity from the rigid body
				float visibleTargetDistance = Vector3.Distance(transform.position, visibleTarget.transform.position);

				float thisSuspicionScore = baseSuspicionAmount * DistanceMultiplyer(visibleTargetDistance, viewRadius, distanceMultiplyer) * VelocityMultiplyer(thisLastKnownVelocity.magnitude, VelocitySensitivity) * Time.deltaTime;

				foreach (RememberedPlayer player in rememberedPlayersData)
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
					RememberedPlayer newPlayer = new RememberedPlayer(visibleTarget, thisSuspicionScore, thisLastKnownTransform, thisLastKnownVelocity);
					rememberedPlayersData.Add(newPlayer);
				}
			}

			// Ads energy to a known locaiton list. keep getting an error not being able to update the list
			if (visibleTarget.GetComponent<Energy>() && !energyLocations.Contains(visibleTarget.transform.position)) energyLocations.Add(visibleTarget.transform.position);

		}

	}

	/// <summary>
	/// Vision strength starts to fade after half distance. Max distance results in 0 -- ie: set your vision distance higher than desired
	/// </summary>
	/// <param name="distance"></param>
	/// <param name="viewMaxDistance"></param>
	/// <param name="distanceSuspicionMultiplyer"></param>
	/// <returns></returns>
	float DistanceMultiplyer(float distance, float viewMaxDistance, float distanceSuspicionMultiplyer)
	{
		
		return (viewMaxDistance - distance) / (viewMaxDistance - viewMaxDistance * 0.5f) * distanceSuspicionMultiplyer;
	}

	float VelocityMultiplyer(float playerVelocity, float velocitySensitivity)
	{
		// velocity magnitude currently not reading correctly
		return 1f;
	}
}
