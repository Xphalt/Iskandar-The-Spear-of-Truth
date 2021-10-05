using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDestructableBush : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "playerSword")
        {
            Destroy(gameObject);
            //insert anything else on bush destruction here
        }
    }
}