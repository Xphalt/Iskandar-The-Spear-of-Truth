using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDestructablePot : MonoBehaviour
{
    void Start()
    {
        //List<Item> item = new List<Item>();

        //adding items to the list
        //item.Add(new Item("Bow", 0));
        //item.Add(new Item("Sword", 1));
        //item.Add(new Item("Shield", 2));
        //item.Add(new Item("Coin", 3));
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "playerSword")
        {
            Destroy(gameObject);
            //instantiate()
            //item.Clear();
            //insert anything else on Pot destruction here
            //instantiate(??? , new Vector3(i * 2.0F, 0, 0), Quaternion.identity);
        }
    }
}
