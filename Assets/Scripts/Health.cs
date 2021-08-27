using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public float maxHealth = 100;
	public float lowHealthThreshold = 30;
	public float healthLossRate = 1;

	public float healthLevel; // public for debug

	private void Start()
	{
		healthLevel = maxHealth;
	}

	public bool isLowHealth()
	{
		return (healthLevel < lowHealthThreshold) ? true : false;
	}

	private void FixedUpdate()
	{
		healthLevel -= healthLossRate * Time.deltaTime;
	}


}
