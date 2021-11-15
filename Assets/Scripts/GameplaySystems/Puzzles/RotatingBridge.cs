using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBridge : MonoBehaviour
{
    public Transform bridge;
    private Vector3 rotatePoint;

    public float rotationSpeed = 45.0f;
    public float rotationAngle = 90;

    private float angleRotated = 90;
    private Collider puzzleCollider;

    private void Start()
    {
        rotatePoint = transform.position;
        rotatePoint.y = bridge.position.y;
        puzzleCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (angleRotated < rotationAngle)
        {
            float newAngle = Mathf.Min(rotationAngle - angleRotated, rotationAngle * Time.deltaTime);
            angleRotated += newAngle;
            transform.parent.Rotate(Vector3.up, newAngle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent(out ThrowSword_Jerzy sword))
        {
            if (!sword.PuzzleHit(puzzleCollider)) angleRotated = 0;
        }
    }
}
