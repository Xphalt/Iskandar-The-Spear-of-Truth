using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : ArcProjectile
{
    public override void EndArc()
    {
        base.EndArc();

        Transform newShroom = transform.GetComponentInChildren<Mushroom>(true).transform;
        if (newShroom)
        {
            newShroom.SetParent(null);
            newShroom.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
