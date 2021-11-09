using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : EnemyBase
{

    public GameObject Explosive;
    [Range(0, 100)]
    public int explosiveDropChance;
    private bool droppedExplosive = false;

    public bool IsDead = false;

    // Update is called once per frame
    public override void Update()
    {
        if (!IsDead)
        {
            base.Update();
        }

        if (stats.health <= 0 || IsDead)
        {
            IsDead = true;
            OnDeathExplosive(explosiveDropChance);
        }
    }

    public void OnDeathExplosive(int chance)
    {
        int percent = Random.Range(0, 101);
        if (percent < chance && !droppedExplosive)
        {
            droppedExplosive = true;
            Instantiate(Explosive, transform.position, Quaternion.identity);
        }
    }
}
