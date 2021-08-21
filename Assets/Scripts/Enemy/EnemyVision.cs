using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : EnemyProcessVision
{
    public Transform eyesightOrigin;
    [Header ("Vision distance and angle of view")]
    [Range(0,360)] public float viewAngle = 110f;
    [Range(1, 100)] public float viewRadius = 15f;
    [Range(1, 5)] public float alertedMultiplier = 2f;
    [HideInInspector]
    public float multilplier = 1f; 

    [Space(5)]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Space(5)]
    [Header("Variables used to control perception abilities")]
    [Range(0, 10)] public float detectionStrength = 1f;
    [Tooltip("Set to less than 1 if eyesight is negatively affected by little or no movement")]
    [Range(0, 1)] public float sensitivityToMovement = 1f;
    [Range(1, 10)] public float distanceSuspicionMultiplyer = 1f;

    [Space(5)]
    [Header("Targets in View")]
    public List<GameObject> visibleTargets = new List<GameObject>();


    private void Start()
	{
        StartCoroutine(FindVisibleTargetsWithDelay(0.3f));
        StartCoroutine(MonitorPlayers(0.3f));

        EnemyLogic.Alerted += HandleAlerted;
        EnemyLogic.Calm += HandleCalm;
    }

	private void OnDestroy()
	{
        EnemyLogic.Alerted -= HandleAlerted;
        EnemyLogic.Calm -= HandleCalm;
    }
	IEnumerator FindVisibleTargetsWithDelay(float delay)
	{
		while (true)
		{
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
		}
	}

    /// <summary>
    /// Finds all players inside a view radius and FOV and sends the List to be procesed.
    /// </summary>
    void FindVisibleTargets()
	{
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(eyesightOrigin.transform.position, viewRadius * multilplier, targetMask);

        for(int i = 0; i < targetsInViewRadius.Length; i++)
		{
            GameObject target = targetsInViewRadius[i].gameObject;
            Vector3 directionToTarget = (target.transform.position - eyesightOrigin.transform.position).normalized;

			if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
			{
                float distanceToTarget = Vector3.Distance(eyesightOrigin.transform.position, target.transform.position);
				if (!Physics.Raycast(eyesightOrigin.transform.position, directionToTarget, distanceToTarget, obstacleMask))
				{
                    if(target.gameObject.GetComponent<CharacterController>()) visibleTargets.Add(target);
				}
			}
        }

        if (visibleTargets.Count > 0) ProcessVision(visibleTargets, viewRadius * multilplier, detectionStrength, distanceSuspicionMultiplyer, sensitivityToMovement);
    }

    public Vector3 directionFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
        if (!angleIsGlobal) angleInDegrees += eyesightOrigin.transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

    void HandleAlerted()
	{
        multilplier = alertedMultiplier;
	}

    void HandleCalm()
    {
        multilplier = 1f;
    }
}
