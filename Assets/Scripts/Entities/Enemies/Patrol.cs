using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [Header("Node Information")]
    //[SerializeField] private string _nodeTag;
    public Transform[] ListOfNodes;
    private int _currentNode;

    protected Animator _myAnimator;
    protected CapsuleCollider _myCapsuleCol;

    protected NavMeshAgent agent;
    private float minRemainingDistance = 0.5f;
    public float pauseTime;
    protected float patrolSpeed;

    protected Rigidbody MyRigid;

    //Vector3 direction;

    public virtual void Start()
    {
        _currentNode = 0;

        MyRigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        _myAnimator = GetComponentInParent<Animator>();
        _myCapsuleCol = GetComponent<CapsuleCollider>();

        agent.autoBraking = false;
        GoToNextNode();

        patrolSpeed = agent.speed;
    }

    public virtual void Update()
    {
        if (agent.enabled && ListOfNodes.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < minRemainingDistance)
            {
                StartCoroutine(Pause(pauseTime));
            }
        }
    }

    void GoToNextNode()
    {
        if (ListOfNodes.Length == 0) return;
        agent.destination = ListOfNodes[_currentNode].position;
        _currentNode = (_currentNode + 1) % ListOfNodes.Length;
    }

    private IEnumerator Pause(float delay) // Temporarily stop the NPC's speed to allow for idle posing
    {
        _myAnimator.SetBool("IsPatrolling", false);
        agent.speed = 0.0f;
        yield return new WaitForSeconds(delay);
        _myAnimator.SetBool("IsPatrolling", true);
        agent.speed = patrolSpeed;
        GoToNextNode();
    }
}
