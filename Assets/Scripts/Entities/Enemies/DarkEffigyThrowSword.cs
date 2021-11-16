using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigyThrowSword : MonoBehaviour
{
    [SerializeField] private float _maxTimeLimit;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _effigyLayer;
    [SerializeField] private LayerMask _boundaryLayer;
    private float _currentTime;
    public bool _returnToSender;
    private Rigidbody _myRigid;

    private EnemyStats stats;
    private PlayerDetection detector;

    private Vector3 _startingPosition;

    void Start()
    {
        _returnToSender = false;
        _myRigid = GetComponent<Rigidbody>();
        _currentTime = 0.0f;
        _myRigid.velocity = transform.forward * _speed;
    }

    void FixedUpdate()
    {
        _startingPosition = transform.parent.position;

        if (_currentTime >= _maxTimeLimit || _returnToSender)
        {
            //_myRigid.velocity = new Vector3(_startingPosition.x, _startingPosition.y, _startingPosition.z) * _speed * Time.deltaTime;

            //Vector3 direction = (transform.position - _startingPosition).normalized;

            //_myRigid.MovePosition(_startingPosition + direction * _speed * Time.deltaTime);

            //transform.position = Vector3.MoveTowards(transform.position, _startingPosition, _speed * Time.deltaTime);

            //_myRigid.velocity = transform.forward * -_speed;

            _currentTime = 0.0f;
        }
        else
            _currentTime += Time.deltaTime;

        if (_returnToSender && (transform.position == _startingPosition))
        {
            print("Position");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            stats.DealDamage(detector.GetCurTarget().GetComponent<StatsInterface>(),
                GetComponentInParent<DarkEffigy>().attackDamages[(int)GetComponentInParent<DarkEffigy>().throwSwordDamage]);
        }
        if (other.gameObject.layer == _effigyLayer && _returnToSender)
        {
            print("Effigy");
            Destroy(gameObject);
        }
        if (other.gameObject.layer == _boundaryLayer)
            _returnToSender = true;
    }
}
