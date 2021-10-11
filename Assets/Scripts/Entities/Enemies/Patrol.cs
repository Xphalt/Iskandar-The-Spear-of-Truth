using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("Node Infomation")]
    [SerializeField] private string _nodeTag;
    public Transform[] ListOfNodes;
    private int _currentNode;

    [Header("Timer Information")]
    [SerializeField] private float _speed;
    [SerializeField] private float _pauseTime;
    [SerializeField] private float _lookSpeed;

    protected bool DefaultToZero = true;
    protected bool Patrolling;
    protected Rigidbody MyRigid;
    
    private Vector3 _direction;
        
    public virtual void Start()
    {
        _currentNode = 0;

        Patrolling = true;

        MyRigid = GetComponent<Rigidbody>();

        transform.LookAt(ListOfNodes[_currentNode].transform);
    }

    public virtual void Update()
    {
        if (Patrolling)
        {
            _direction = (ListOfNodes[_currentNode].position - transform.position).normalized;
            MyRigid.velocity = _direction * _speed;
        }
        else if (DefaultToZero)
        {
            MyRigid.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == _nodeTag)
        {
            StartCoroutine(Pause(_pauseTime));
            StartCoroutine(Look(_pauseTime));
            _currentNode++;
            _currentNode %= ListOfNodes.Length;
        }
    }

    private IEnumerator Pause(float delay) // Temporarily stop the NPC's speed to allow for idle posing
    {
        float currentSpeed = _speed;

        _speed = 0.0f;
        yield return new WaitForSeconds(delay);
        _speed = currentSpeed;
    }

    private IEnumerator Look(float delay) // During the NPC's pause turn to face the next node
    {
        float timer = 0.0f;

        yield return new WaitForSeconds(delay / 2); // To allow the NPC an average looking time the pause time variable will be halved

        while (timer < delay)
        {
            timer += Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(ListOfNodes[_currentNode].position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timer);

            yield return null;
        }

    }
}
