using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    public float hearingDistance = 10;
    public Vector3 soundLocation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        RoosBetterCharacterController.PlayerMovingSound += HandlePlayerSound;
    }

	private void OnDestroy()
	{
        RoosBetterCharacterController.PlayerMovingSound -= HandlePlayerSound;
    }

    public bool hasHearingLocation()
	{
        return (soundLocation == Vector3.zero) ? false : true; // cant null a struct so using vector3.zero as my null
	}

    void HandlePlayerSound(Vector3 soundOrigin)
	{
        soundLocation = (Vector3.Distance(transform.position, soundOrigin) <= hearingDistance) ? soundOrigin : Vector3.zero;
	}
}
