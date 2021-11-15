using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigyThrowSword : MonoBehaviour
{
    [SerializeField] private float _maxTimeLimit;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _effigyLayer;
    private float _currentTime;
    private bool _returnToSender;
    private Rigidbody _myRigid;

    private EnemyStats stats;
    private PlayerDetection detector;

    private Vector3 _startingPosition;

    public bool _swordReturned;

    void Start()
    {
        _swordReturned = false;
        _returnToSender = false;
        _startingPosition = transform.position;
        _myRigid = GetComponent<Rigidbody>();
        _currentTime = 0.0f;
        _myRigid.velocity = transform.forward * _speed;
    }

    void FixedUpdate()
    {
        if (_currentTime >= _maxTimeLimit || _returnToSender)
        {
            _myRigid.velocity = -transform.forward * _speed;
            _currentTime = 0.0f;
        }
        else
            _currentTime += Time.deltaTime;

        if (_returnToSender && (transform.position == _startingPosition))
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(),
                GetComponentInParent<DarkEffigy>().attackDamages[(int)GetComponentInParent<DarkEffigy>().throwSwordDamage]);
        }
        if (other.gameObject.layer == _effigyLayer)
        {
            _swordReturned = true;
            Destroy(gameObject);
        }
        _returnToSender = true;
    }
}
