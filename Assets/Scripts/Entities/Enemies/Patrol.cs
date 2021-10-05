using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    private NavMeshAgent agent;
    private float minRemainingDistance;
    public Transform[] nodes;
    public float speed;

    protected bool Patrolling;
    int currentNode;

    protected Rigidbody MyRigid;

    Vector3 direction;

    [SerializeField]
    private string nodeTag;

    public bool defaultToZero = true;

    public virtual void Start()
    {
        currentNode = 0;

        Patrolling = true;

        MyRigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GoToNextNode();
    }

    public virtual void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < minRemainingDistance)
        {
            GoToNextNode();
        }

        //if (Patrolling)
        //{
        //    direction = (nodes[currentNode].position - transform.position).normalized;

        //    MyRigid.velocity = direction * speed;
        //    transform.rotation = Quaternion.LookRotation(MyRigid.velocity);
        //}
        //else if (defaultToZero)
        //{
        //    MyRigid.velocity = Vector3.zero;
        //}
    }

    void GoToNextNode()
    {

        agent.destination = nodes[currentNode].position;
        currentNode = (currentNode + 1) % nodes.Length;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == nodeTag)
    //    {
    //        currentNode++;
    //        currentNode %= nodes.Length;
    //    }
    //}
}
