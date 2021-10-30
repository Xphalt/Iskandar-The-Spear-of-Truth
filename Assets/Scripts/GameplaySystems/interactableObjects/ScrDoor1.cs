using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//morgan

public class ScrDoor1 : MonoBehaviour
{
    float height = 3;

    public int id;

    public float Xpos;
    public float Ypos;
    public float Zpos;

    private void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
        Xpos = transform.position.x;
        Ypos = transform.position.y;
        Zpos = transform.position.z;
    }


    private void OnDoorwayOpen(int id)
    {
        if (id == this.id)
        {
            transform.position = new Vector3(Xpos, Ypos * height, Zpos);
        }
    }

    private void OnDoorwayClose(int id)
    {
        if (id == this.id)
        {
            transform.position = new Vector3(Xpos, Ypos, Zpos);
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
    }
}
