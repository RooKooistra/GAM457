using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public float maxHealth = 100;
	public float lowHealthThreshold = 30;
	public float healthLossRate = 1;

	public float healthLevel;

	DebugInfo debugInfo;

	private void Start()
	{
		debugInfo = GetComponent<DebugInfo>();
		healthLevel = maxHealth;
	}

	public bool isLowHealth()
	{
		return (healthLevel < lowHealthThreshold) ? true : false;
	}

	private void FixedUpdate()
	{
		healthLevel = (healthLevel < 0) ? 0 : healthLevel -= healthLossRate * Time.deltaTime;
		if(debugInfo!=null) debugInfo.UpdateText(debugInfo.healthText, Mathf.RoundToInt(healthLevel).ToString());

		if (healthLevel == 0) Destroy(gameObject);
	}




}
