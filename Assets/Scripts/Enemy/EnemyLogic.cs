using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyLogic : MonoBehaviour
{
    public List<Player> players = new List<Player>();

	public Text dbText;

	[Range(0, 20)] public float cooldownRate = 10f;

	public static event Action Alerted;
	public static event Action Calm;

	// Monitors player list to see if any are visible
	public IEnumerator MonitorPlayers(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			if(players.Count > 0)
			{
				float totalSuspicionCount = 0;

				foreach (Player player in players)
				{
					float suspicion = (player.suspicionScore - cooldownRate * Time.deltaTime > 0f) ? (player.suspicionScore - cooldownRate * Time.deltaTime > 1f)? 1: player.suspicionScore - cooldownRate * Time.deltaTime : 0f;
					player.suspicionScore = suspicion;

					if (suspicion >= 1f) Alerted();
					totalSuspicionCount += suspicion;

					string sussText = (suspicion > 0) ? (suspicion > 1) ? "SEEN" : Mathf.Round(suspicion * 100).ToString()+"%" : "None";
					dbText.text = sussText;
				}

				if (totalSuspicionCount == 0) Calm();
			}

		}
	}

}
