using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyVision))]
public class EnemyVisionEditor : Editor
{
	private void OnSceneGUI()
	{
		EnemyVision vision = (EnemyVision)target;
		Handles.color = Color.cyan;
		Handles.DrawWireArc(vision.eyesightOrigin.transform.position, Vector3.up, Vector3.forward, 360, vision.viewRadius * vision.multilplier);

		Vector3 viewAngleLeft = vision.directionFromAngle(-vision.viewAngle / 2f, false);
		Vector3 viewAngleRight = vision.directionFromAngle(vision.viewAngle / 2f, false);

		Handles.DrawLine(vision.eyesightOrigin.transform.position, vision.eyesightOrigin.transform.position + viewAngleLeft * vision.viewRadius * vision.multilplier);
		Handles.DrawLine(vision.eyesightOrigin.transform.position, vision.eyesightOrigin.transform.position + viewAngleRight * vision.viewRadius * vision.multilplier);

		Handles.color = Color.magenta;
		foreach (GameObject visibleTarget in vision.visibleTargets)
		{
			Handles.DrawLine(vision.eyesightOrigin.transform.position, visibleTarget.transform.position);
		}
	}
}
