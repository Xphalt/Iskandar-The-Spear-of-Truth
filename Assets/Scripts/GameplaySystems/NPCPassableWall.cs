using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPassableWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreLayerCollision(0, 0);
    }
}
