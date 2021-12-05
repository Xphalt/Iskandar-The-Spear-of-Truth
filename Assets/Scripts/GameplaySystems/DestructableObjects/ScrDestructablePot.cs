using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDestructablePot : MonoBehaviour
{
    private EntityDrop drop;

    public float lootHeightOffset;

    public float lootHeightForce;
    public float lootForwardForce;

    const float LOWER_RANDOM_ANGLE_BOUNDARY = 0.3f;
    const float UPPER_RANDOM_ANGLE_BOUNDARY = 0.8f;

    public float ID;
    public bool destroyed = false;

    private void Start()
    {
        drop = GetComponent<EntityDrop>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("playerSword"))
        {
            destroyed = true;
            drop.SpawnLoot();
        }
    }

    private void Update()
    {
        if (destroyed == true)
        {
            this.GetComponent<Renderer>().enabled = false;
            var colliders = GetComponents<BoxCollider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }
    
    public void SetForces(GameObject obj)
    {
        obj.transform.position = obj.transform.position + new Vector3(0, lootHeightOffset, 0);
        obj.transform.Rotate(0, Random.Range(0,360), 0);
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.up * Random.Range(LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootHeightForce);
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * Random.Range(LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootForwardForce);
    }
}