using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectile : MonoBehaviour
{
    protected Rigidbody myRigid;

    protected Vector3 start, end, centre;
    public Vector3 rotVec;
    protected float timer = 0, duration;

    protected bool moving = false;

    public float arcSize = 5, rotSpeed = 360;

    public virtual void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (moving)
        {
            timer += Time.deltaTime;
            if (timer > duration) EndArc();
            else TraverseArc();
        }
    }

    public virtual void StartArc(Vector3 _start, Vector3 _end, float _speed)
    {
        transform.SetParent(null);
        transform.position = _start;
        gameObject.SetActive(true);
        start = _start;
        end = _end;
        centre = Vector3.Lerp(start, end, 0.5f) - Vector3.up * arcSize;
        duration = (end - start).magnitude / _speed;
        moving = true;
    }

    public virtual void TraverseArc()
    {
        myRigid.MovePosition(Vector3.Slerp(start - centre, end - centre, timer / duration) + centre);
        transform.Rotate(rotSpeed * Time.deltaTime * rotVec);
    }

    public virtual void EndArc()
    {
        moving = false;
    }
}
