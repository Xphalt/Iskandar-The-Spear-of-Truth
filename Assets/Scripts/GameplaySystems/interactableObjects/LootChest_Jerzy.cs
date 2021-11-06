using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest_Jerzy : MonoBehaviour
{

    public bool isInteractable;
    public GameObject lootChestLidPivot;
    public int minGems;
    public int maxGems;
    public GameObject GemPrefab;
    public float gemHeightOffset;

    public float lootHeightForce;
    public float lootForwardForce;
    public float lootSideForce;

    const float LOWER_RANDOM_ANGLE_BOUNDARY = 0.5f;
    const float UPPER_RANDOM_ANGLE_BOUNDARY = 0.7f;


    void Start()
    {
        isInteractable = true;
    }


    public void Interact()
    {
        if(isInteractable)
        {
            GetComponent<EntityDrop>().SpawnLoot();

            lootChestLidPivot.GetComponent<Animator>().Play("LootChestLidOpening");

            //generate loot (currently only gems)
            int amountOfGems = Random.Range(minGems, maxGems);

            //display loot
            for (int gems = 0; gems < amountOfGems; gems++)
            {

                GameObject obj = Instantiate(GemPrefab, transform.position + new Vector3(0, 0, 0), transform.rotation);
                SetForces(obj);


            }


            isInteractable = false;
            gameObject.layer = 0;
        }

    }

    public void SetForces(GameObject obj)
    {
        obj.transform.position = obj.transform.position + new Vector3(0,gemHeightOffset,0);
        obj.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootHeightForce);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootForwardForce);
        obj.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-LOWER_RANDOM_ANGLE_BOUNDARY, UPPER_RANDOM_ANGLE_BOUNDARY) * lootSideForce);
    }

}
