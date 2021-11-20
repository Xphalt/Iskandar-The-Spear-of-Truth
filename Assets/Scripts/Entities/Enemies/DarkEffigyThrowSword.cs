using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigyThrowSword : MonoBehaviour
{
    [SerializeField] private float _maxTimeLimit;
    [SerializeField] private float _speed;
    [SerializeField] private string _playerTag;
    [SerializeField] private string _effigyTag = "DarkEffigy";
    [SerializeField] private string _boundaryTag;
    private float _currentTime;
    public bool _returnToSender;
    private Rigidbody _myRigid;

    //private PlayerDetection detector;

    private Vector3 _startingPosition;

    private void OnEnable()
    {
        _returnToSender = false;
        _myRigid = GetComponent<Rigidbody>();
        _currentTime = 0.0f;
        _myRigid.velocity = transform.forward * _speed;
    }

    private void OnDisable()
    {
        transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        transform.localPosition = new Vector3(0.0f, 1.2f, 0.0f);
    }

    void FixedUpdate()
    {
        _startingPosition = transform.parent.position;

        if (_currentTime >= _maxTimeLimit || _returnToSender)
        {
            _returnToSender = true;

            Vector3 direction = (_startingPosition - transform.position).normalized;

            _myRigid.velocity = (direction * _speed);

            _currentTime = 0.0f;
        }
        else
            _currentTime += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats _playerStats))
            _playerStats.TakeDamage(GetComponentInParent<DarkEffigy>().throwSwordDamage);

        if (other.gameObject.CompareTag(_effigyTag) && _returnToSender)
        {
            _returnToSender = false;
            gameObject.SetActive(false);
            GetComponentInParent<Animator>().SetTrigger("SwordReturn");
        }

        if (other.CompareTag(_boundaryTag) || other.CompareTag(_playerTag))
            _returnToSender = true;
    }
}
