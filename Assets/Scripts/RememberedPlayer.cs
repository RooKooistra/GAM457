using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberedPlayer
{
    public GameObject playerGameobject;
    public float suspicionScore;
    public Transform lastKnownTransform;
    public Vector3 lastKnownVelocity;

    public RememberedPlayer(GameObject _playerGameobject, float _suspicionScore, Transform _lastKnownTransform, Vector3 _lastKnownVelocity)
    {
        playerGameobject = _playerGameobject;
        suspicionScore = _suspicionScore;
        lastKnownTransform = _lastKnownTransform;
        lastKnownVelocity = _lastKnownVelocity;
    }
}
