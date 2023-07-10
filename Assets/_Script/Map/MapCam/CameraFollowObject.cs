using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.5f;
    [SerializeField] private Movement movement;

    private int facingDirection;
    private Vector3 previousTargetPosition;


    private void Awake()
    {
        movement.OnFlip += HandleFlip;
        previousTargetPosition = target.position;
        facingDirection = movement.FacingDirection;
    }

    private void Update()
    {
        if (target.position != previousTargetPosition)
        {
            transform.position = target.position;
            previousTargetPosition = target.position;
        }
    }

    private void HandleFlip()
    {
        LeanTween.rotateY(gameObject, DeterminEndRotation(), smoothSpeed);
    }

    private float DeterminEndRotation()
    {
        if (movement.FacingDirection == 1)
            return 0f;
        else
            return 180f;
    }
}
