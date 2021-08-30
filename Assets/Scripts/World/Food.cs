using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
	float cooldownLimit;
	float cooldown = 0f;
	bool isEnabled;
	public Light lighting;

	private void Start()
	{
		cooldownLimit = Random.Range(30, 60);
	}

	private void Update()
	{
		isEnabled = false;
		cooldown += Time.deltaTime;
		if (cooldown > cooldownLimit) isEnabled = true;

		lighting.enabled = isEnabled;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<RoosBetterCharacterController>() && isEnabled)
		{
			other.GetComponent<Health>().healthLevel = other.GetComponent<Health>().maxHealth;
			cooldownLimit = Random.Range(30, 60);
			cooldown = 0f;
		}	
	}
}
