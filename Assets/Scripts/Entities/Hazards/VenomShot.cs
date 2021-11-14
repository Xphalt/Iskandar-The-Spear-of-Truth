// Script made by Jerzy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomShot : MonoBehaviour
{
    private float projectileSpeed;
    private Vector3 projectileDirection;
    Rigidbody m_Rigidbody;
    private float timeExisted;
    private const float LIFE_TIME = 2;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_Rigidbody.velocity = projectileDirection * projectileSpeed;
        timeExisted += Time.deltaTime;
        if (timeExisted >= LIFE_TIME)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction, float speed)
    {
        projectileDirection = direction;
        projectileSpeed = speed;
    }
}
