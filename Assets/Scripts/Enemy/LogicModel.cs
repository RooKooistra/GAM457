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

	[Tooltip("Cooldown rate for calculating player detection")]
	[Range(0, 20)] public float cooldownRate = 10f;
	[Tooltip("Last known position is forgoten after this many seconds")]
	public float memoryCooldownSeconds = 30f;

	public bool canSeePlayer = false;
	public bool canSeeEnergy = false;

	public event Action Alerted;
	public event Action Calm;

	public static event Action<GameObject> FireGuns;
	public static event Action StopGuns;

	private void Start()
	{
		StartCoroutine(SetCalmDelayed(0.1f)); // had an error if Calm() was in start
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

	public bool knowEnergyLocation()
	{
		return (energyLocations.Count > 0) ? true : false;
	}

	public bool remembersPlayerLocation()
	{
		float memCount = 0;
		foreach (RememberedPlayer player in rememberedPlayersData)
		{
			memCount += player.memoryCooldown;
		}

		return (memCount == 0) ? false : true;
	}

	public Vector3 GetClosestSuspicious(float suspiciousThreshold)
	{
		List<Vector3> listToProcess = new List<Vector3>();
		foreach (RememberedPlayer player in rememberedPlayersData)
		{
			if (player.suspicionScore > suspiciousThreshold) listToProcess.Add(player.playerGameobject.transform.position);
		}
		Vector3 closestPosition = GetClosestPosition(listToProcess);
		return (closestPosition == null) ? Vector3.zero : closestPosition;
	}

	/// <summary>
	/// gets all players with a suspicion score of over 50% and returns the closest one
	/// </summary>
	/// <returns></returns>
	public Vector3 GetClosestLastPlayerPosition()
	{
		List<Vector3> listToProcess = new List<Vector3>();
		foreach(RememberedPlayer player in rememberedPlayersData)
		{
			if (player.lastKnownTransform != null) listToProcess.Add(player.lastKnownTransform.position);
		}
		Vector3 closestPosition = GetClosestPosition(listToProcess);
		return (closestPosition == null) ? Vector3.zero : closestPosition;
	}

	public bool checkNextToEnergy()
	{
		return (energyLocations.Count == 0) ? false :
			(Vector3.Distance(transform.position, GetClosestPosition(energyLocations)) < 1f) ? true : false;
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
			float totalSuspicionCount = 0; // used to calculate calm state

			foreach (RememberedPlayer player in rememberedPlayersData)
			{
				player.memoryCooldown = ((player.memoryCooldown -= 10 * Time.deltaTime) < 0) ? 0 : player.memoryCooldown -= 10 * Time.deltaTime;
				if (player.memoryCooldown == 0) player.lastKnownTransform = null;

				float suspicion = (player.suspicionScore - cooldownRate * Time.deltaTime > 0f) ? (player.suspicionScore - cooldownRate * Time.deltaTime > 1f) ? 1 : player.suspicionScore - cooldownRate * Time.deltaTime : 0f;
				player.suspicionScore = suspicion;

				if (suspicion >= 0.99f)
				{
					player.memoryCooldown = memoryCooldownSeconds;
					canSeePlayer = true;
					Alerted?.Invoke();

					FireGuns?.Invoke(player.playerGameobject);
				}
				else
				{
					StopGuns?.Invoke();
				}

				totalSuspicionCount += suspicion;

				string sussText = (suspicion > 0) ? (suspicion > .99) ? "SEEN" : Mathf.Round(suspicion * 100).ToString() + "%" : "None";
				if (dbText != null) dbText.text =sussText;
			}

			if (totalSuspicionCount == 0) Calm?.Invoke();
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
		canSeeEnergy = false;
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
					RememberedPlayer newPlayer = new RememberedPlayer(visibleTarget, thisSuspicionScore, thisLastKnownTransform, thisLastKnownVelocity, 0f);
					rememberedPlayersData.Add(newPlayer);
				}
			}

			// Ads energy to a known locaiton list. keep getting an error not being able to update the list
			if (visibleTarget.GetComponent<Energy>())
			{
				if(!energyLocations.Contains(visibleTarget.transform.position)) energyLocations.Add(visibleTarget.transform.position);
				canSeeEnergy = true;
			}
		
			if (visibleTarget.GetComponent<Food>() && !POILocations.Contains(visibleTarget.transform.position)) POILocations.Add(visibleTarget.transform.position);

		}

	}

	public Vector3 GetClosestPosition(List<Vector3> list)
	{
		float closestItemSoFar = float.MaxValue;
		Vector3 positionToReturn = Vector3.zero;

		foreach(Vector3 item in list)
		{
			float distanceToThisItem = Vector3.Distance(transform.position, item);
			if(distanceToThisItem < closestItemSoFar)
			{
				positionToReturn = item;
				closestItemSoFar = distanceToThisItem;
			}
		}
		return positionToReturn;
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
