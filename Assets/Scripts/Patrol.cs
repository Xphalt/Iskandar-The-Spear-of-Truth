using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
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
    }

    public virtual void Update()
    {
        if (Patrolling)
        {
            direction = (nodes[currentNode].position - transform.position).normalized;

            MyRigid.velocity = direction * speed;
            transform.rotation = Quaternion.LookRotation(MyRigid.velocity);
        }
        else if (defaultToZero)
        {
            MyRigid.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == nodeTag)
        {
            currentNode++;
            currentNode %= nodes.Length;
        }
    }
}
