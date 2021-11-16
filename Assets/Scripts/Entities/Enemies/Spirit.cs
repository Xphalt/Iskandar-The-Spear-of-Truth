using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : EnemyBase
{
    public GameObject DeathExplosive;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

    }

    public void DeathDrop()
    {
        Instantiate(DeathExplosive);
    }
    
}
