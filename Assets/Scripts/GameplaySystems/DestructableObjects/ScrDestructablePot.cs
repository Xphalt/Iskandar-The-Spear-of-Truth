using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDestructablePot : MonoBehaviour
{
    private EntityDrop drop;

    private void Start()
    {
        drop = GetComponent<EntityDrop>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("playerSword"))
        {
            drop.SpawnLoot();
            Destroy(gameObject);
        }
    }
}
