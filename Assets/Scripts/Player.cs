using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public GameObject playerGameobject;
    public float suspicionScore;
    public Transform lastKnownTransform;
    public Vector3 lastKnownVelocity;

    public Player(GameObject _playerGameobject, float _suspicionScore, Transform _lastKnownTransform, Vector3 _lastKnownVelocity)
    {
        playerGameobject = _playerGameobject;
        suspicionScore = _suspicionScore;
        lastKnownTransform = _lastKnownTransform;
        lastKnownVelocity = _lastKnownVelocity;
    }
}
