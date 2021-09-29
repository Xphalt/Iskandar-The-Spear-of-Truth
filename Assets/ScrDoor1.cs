using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//morgan

public class ScrDoor1 : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //insert door open animation here
            print("success");
        }
    }
}
