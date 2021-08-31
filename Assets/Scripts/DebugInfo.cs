using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Anthill.AI;

public class DebugInfo : MonoBehaviour
{
	public Text suspicionText;
	public Text plannerText;
	public Text healthText;

	AntAIAgent antAgent;

	private void Start()
	{
		antAgent = GetComponent<AntAIAgent>();
	}
	public void UpdateText(Text debugText, string textString)
	{
		debugText.text = textString;
	}

	private void FixedUpdate()
	{
		// get the current active state
		GameObject activeObject = null; 
		Transform[] children = GetComponentsInChildren<Transform>();
		foreach(Transform child in children)
		{
			if (child.tag == "Scout") continue;
			if (child.gameObject.activeSelf == true) activeObject = child.gameObject;
		}

		if (antAgent != null && activeObject != null) UpdateText(plannerText, activeObject.name.ToString());
	}
}


