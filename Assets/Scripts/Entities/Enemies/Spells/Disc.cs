using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    private Transform _player;
    private float _discSpeed, _discDamage, _discOffset;

    private int _numberOfBounces;
    public int _maxNumberOfBounces;

    private Rigidbody _myRigid;

    private Animator _myAnimator;

    void Awake()
    {
        _myRigid = GetComponent<Rigidbody>();
        _myAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision other)
    {
        ++_numberOfBounces;
        if (other.transform.TryGetComponent(out PlayerStats stats))
        {
            stats.TakeDamage(_discDamage);
            DisableDisc();
        }
        else
        {
            if (_numberOfBounces == _maxNumberOfBounces)
                DisableDisc();
            else
            {
                UpdatePlayerLocation();
            }
        }
    }

    private void DisableDisc()
    {
        _myRigid.velocity = Vector3.zero;
        _myAnimator.SetTrigger("Ded");
    }

    public void SetVariables(Transform _target, float _speed, float _damage, float _offset)
    {
        _player = _target;
        _discSpeed = _speed;
        _discDamage = _damage;        
        UpdatePlayerLocation(_offset);
    }

    private void UpdatePlayerLocation(float _offset = 0)
    {
        transform.LookAt(_player);
        transform.Rotate(Vector3.up * _offset);
        _myRigid.velocity = transform.forward * _discSpeed;
    }

    private void DeathTime()
    {
        Destroy(gameObject);
    }
}
