using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats stats))
        {
            //Get items left over
            GroundItem[] groundObjs = FindObjectsOfType<GroundItem>();
            foreach (GroundItem item in groundObjs)
            {
                if (item.itemobj.objType != ObjectType.Resource)
                {
                    if (item.itemobj.data.name.entryReference == "Health Drop") 
                        Destroy(item.gameObject); 
                    if (stats.equipment.GetSlots[(int)EquipSlot.ItemSlot].item.id == item.itemobj.data.id)
                    {
                        stats.equipment.GetSlots[(int)EquipSlot.ItemSlot].AddAmount(1);
                        Destroy(item.gameObject);
                    }
                    else if (stats.inventory.AddItem(new Item(item.itemobj), 1))
                        Destroy(item.gameObject);  //Only if the item is picked up 
                }
                else //It's a resource
                {
                    if (((ResourceObject)(item.itemobj)).resourceType == ResourceType.RevivalGem)
                    {
                        if (stats.inventory.FindItemOnInventory(item.itemobj.data) != null)
                        { }
                        else if (stats.inventory.AddItem(new Item(item.itemobj), 1))
                            Destroy(item.gameObject);
                    }
                    else if ((((ResourceObject)(item.itemobj)).resourceType == ResourceType.Gems)) 
                        Destroy(item.gameObject); 
                    else if (stats.inventory.AddItem(new Item(item.itemobj), 1))
                        Destroy(item.gameObject);  //Only if the item is picked up  
                }
            } 

            stats.SaveStats();

            // Dominique, Use fade to black screen
            UIManager.instance.GetBlackoutScreen().gameObject.SetActive(true);
            UIManager.instance.GetBlackoutScreen().FadeOutOfScene(sceneIndex);
        }
    }
}
