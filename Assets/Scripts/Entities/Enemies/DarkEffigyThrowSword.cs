using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigyThrowSword : MonoBehaviour
{
    [SerializeField] private float _maxTimeLimit;
    [SerializeField] private float _speed;
    private float _currentTime;
    private bool _returnToSender;
    private Rigidbody _myRigid;

    private Vector3 _startingPosition;

    void Start()
    {
        _returnToSender = false;
        _startingPosition = transform.position;
        _myRigid = GetComponent<Rigidbody>();
        _currentTime = 0.0f;
    }

    void FixedUpdate()
    {
        _currentTime += Time.deltaTime;

        _returnToSender = (_currentTime > _maxTimeLimit) ? true : false;

        if (!_returnToSender)
        {
            _myRigid.velocity = transform.forward * _speed;
            _currentTime = 0.0f;
        }
        else
        {
            _myRigid.velocity = transform.forward * -_speed;
            _currentTime = 0.0f;
        }

        if (_returnToSender && (transform.position == _startingPosition))
            gameObject.SetActive(false);
    }
}
