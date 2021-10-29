using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject_Sal : ItemObject_Sal
{
    public Defencevalues defValues = new Defencevalues();

    public string Desc;

    public void Awake()
    {
        type = ItemType.Armor;
        Desc = string.Concat(
            "<b><color=red>Physical Defence</color></b>: ", defValues.physicalDef,
            "\n<b><color=red>Poison Protection</color></b>: ", defValues.poisonProtection.ToString() ,
            "\n<b><color=red>Desert Protection</color></b>: ", defValues.desertProtection.ToString() ,
            "\n<b><color=red>Snow Protection</color></b>: ", defValues.snowProtection.ToString());
    }

    public override void UseAfter()
    {
        
    }

    public override void UseBefore()
    {
        Debug.Log("Armor used/Equipped");
    } 
}
