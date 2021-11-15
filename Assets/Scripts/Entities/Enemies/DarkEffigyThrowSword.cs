using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEffigyThrowSword : MonoBehaviour
{
    [SerializeField] private float _throwSpeed;
    [SerializeField] private float _spinSpeed;
    [SerializeField] private float _maxThrowDistance;

    private Vector3 _startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * _spinSpeed, Space.Self);
    }
}
