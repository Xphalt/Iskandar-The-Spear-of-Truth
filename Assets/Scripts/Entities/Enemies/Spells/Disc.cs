using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private Transform _player;
    private float _discSpeed, _discDamage;
    private Vector3 _discDirection;

    private bool _hasBounced;

    private Rigidbody _myRigid;

    private Vector3 _discPosition;

    void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        _hasBounced = false;
    }

    private void OnEnable()
    {
        UpdatePlayerLocation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _player)
            Destroy(gameObject);
        else
        {
            if (_hasBounced)
                Destroy(gameObject);
            else
            {
                UpdatePlayerLocation();
                _hasBounced = true;
            }
        }
    }

    public void SetVariables(Transform _target, float _speed, float _damage, Vector3 _startPosition)
    {
        _player = _target;
        _discSpeed = _speed;
        _discDamage = _damage;
        _discPosition = _startPosition;
        UpdatePlayerLocation();
    }

    private void UpdatePlayerLocation()
    {
        transform.rotation = Quaternion.LookRotation(_player.position - transform.position);
        _myRigid.velocity = transform.forward * _discSpeed;
    }
}
