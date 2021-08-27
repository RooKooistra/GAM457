using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberedPlayer
{
    public GameObject playerGameobject;
    public float suspicionScore;
    public Transform lastKnownTransform;
    public Vector3 lastKnownVelocity;
    public float memoryCooldown;

    public RememberedPlayer(GameObject _playerGameobject, float _suspicionScore, Transform _lastKnownTransform, Vector3 _lastKnownVelocity, float _memoryCooldown)
    {
        playerGameobject = _playerGameobject;
        suspicionScore = _suspicionScore;
        lastKnownTransform = _lastKnownTransform;
        lastKnownVelocity = _lastKnownVelocity;
        memoryCooldown = _memoryCooldown;
    }
}
