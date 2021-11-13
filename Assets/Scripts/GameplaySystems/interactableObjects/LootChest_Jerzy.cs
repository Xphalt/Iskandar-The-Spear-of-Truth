using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest_Jerzy : MonoBehaviour
{

    public bool isInteractable;
    public GameObject lootChestLidPivot;
    public float lootHeightOffset;

    public float lootHeightForce;
    public float lootForwardForce;
    public float lootSideForce;

    const float LOWER_RANDOM_ANGLE_BOUNDARY = 0.3f;
    const float UPPER_RANDOM_ANGLE_BOUNDARY = 0.8f;

    public Vector3 ID;

    void Start()
    {
        isInteractable = true;
        ID = this.transform.position;
        print(this.transform.GetSiblingIndex());
    }

    public void Interact()
    {
        if(isInteractable)
        {
            GetComponent<EntityDrop>().SpawnLoot();

            lootChestLidPivot.GetComponent<Animator>().Play("LootChestLidOpening");

            isInteractable = false;
            gameObject.layer = 0;
        }

    }

    public void SetForces(GameObject obj)
    {
        obj.transform.position = obj.transform.position + new Vector3(0, lootHeightOffset, 0);
        obj.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootHeightForce);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootForwardForce);
        obj.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-UPPER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootSideForce);
    }

}
