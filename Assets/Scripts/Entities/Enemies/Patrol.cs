using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [Header("Node Information")]
    public Transform[] ListOfNodes;
    private int _currentNode;
    protected bool isPaused;

    protected Animator _myAnimator;
    protected CapsuleCollider _myCapsuleCol;

    protected NavMeshAgent agent;
    private float minRemainingDistance = 0.5f;
    public float pauseTime;
    protected float patrolSpeed;

    protected Rigidbody MyRigid;

    [SerializeField] private bool _stopAtLastWaypoint;
    private bool _noMorePatrolling;

    [SerializeField] private MonoBehaviour _scriptToEnable;

    public virtual void Start()
    {
        _currentNode = 0;

        MyRigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        _myAnimator = GetComponentInParent<Animator>();
        _myAnimator.SetBool("IsPatrolling", ListOfNodes.Length > 0);
        _myCapsuleCol = GetComponent<CapsuleCollider>();

        agent.autoBraking = false;
        GoToNextNode();

        patrolSpeed = agent.speed;

        _noMorePatrolling = false;
    }

    public virtual void Update()
    {
        if (agent.enabled && ListOfNodes.Length > 0 && !_noMorePatrolling)
        {
            if ((!agent.pathPending && agent.remainingDistance < minRemainingDistance) && !isPaused)
            {
                StartCoroutine(Pause(pauseTime));
            }
        }
    }

    void GoToNextNode()
    {
        if (ListOfNodes.Length == 0) return;

        if (_stopAtLastWaypoint && _currentNode == ListOfNodes.Length)
        {
            _myAnimator.SetBool("IsPatrolling", false);
            _noMorePatrolling = true;
            agent.speed = 0.0f;

            _scriptToEnable.enabled = true;
            this.enabled = false;
        }
        else
        {
            agent.destination = ListOfNodes[_currentNode].position;
            ++_currentNode;
            if (!_stopAtLastWaypoint)
            {
                _currentNode = _currentNode % ListOfNodes.Length;
            }
        }
    }

    private IEnumerator Pause(float delay) // Temporarily stop the NPC's speed to allow for idle posing
    {
        _myAnimator.SetBool("IsPatrolling", false);
        agent.speed = 0.0f;
        isPaused = true;
        yield return new WaitForSeconds(delay);
        _myAnimator.SetBool("IsPatrolling", true);
        agent.speed = patrolSpeed;
        isPaused = false;
        GoToNextNode();
    }
}
