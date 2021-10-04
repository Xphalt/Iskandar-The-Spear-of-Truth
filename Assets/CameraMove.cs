using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float Row, Line;
    private Rigidbody rigi;
    void Start()
    {
        rigi = GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        Row = Input.GetAxis("Horizontal");
        Line = Input.GetAxis("Vertical");
        Vector3 MoveVec = new Vector3(Row, 0, Line);

        if (MoveVec.magnitude > 1)
        {
            MoveVec = MoveVec.normalized;
        }

        rigi.velocity = MoveVec * 2;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            rigi.velocity = Vector3.zero;
        }
        else
        {
            Physics.IgnoreCollision(collision.transform.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
