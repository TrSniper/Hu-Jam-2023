using System;
using UnityEngine;

public class PlayerFollowTarget : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float zeroGravityLerpSpeed = 0.1f;

    [Header("Info - No Touch")] public float lerpSpeed = 1;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        GravityManager.OnGravityChanged += ChangeLerpSpeed;
    }

    //Player moves in FixedUpdate() so we have to follow in FixedUpdate() but change script execution order
    //Default Time -> PlayerFollowTarget -> CameraController
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, lerpSpeed);
    }

    private void ChangeLerpSpeed(bool isGravityActive)
    {
        if (isGravityActive) lerpSpeed = 1;
        else lerpSpeed = zeroGravityLerpSpeed;
    }

    private void OnDestroy()
    {
        GravityManager.OnGravityChanged -= ChangeLerpSpeed;
    }
}
