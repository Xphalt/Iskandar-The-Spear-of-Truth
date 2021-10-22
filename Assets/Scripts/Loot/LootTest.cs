using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class LootTest : MonoBehaviour
{
    PlayerActionsAsset input;

    private float currentTime = 0.0f;
    private float interval = 5.0f;

    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= interval)
        {
            FindObjectOfType<EntityDrop>().SpawnLoot();
            Debug.Log(string.Concat("Tentative Number: ", DropSystem.Instance.tentativeNum[EntityType.type1]));

            currentTime = .0f;
        }
    }
}
