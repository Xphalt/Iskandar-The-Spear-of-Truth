using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : EnemyBase
{
    public GameObject Explosive;
    [Range(0, 100)]
    public int explosiveDropChance;
    private bool droppedExplosive = false;

    public void OnDeathExplosive()
    {
        int percent = Random.Range(0, 101);
        if (percent < explosiveDropChance && !droppedExplosive)
        {
            droppedExplosive = true;
            Instantiate(Explosive, transform.position, Quaternion.identity);
        }
    }
}
