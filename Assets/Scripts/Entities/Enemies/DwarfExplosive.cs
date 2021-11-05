using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfExplosive : MonoBehaviour
{

    public float Countdown;


    void Update()
    {
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            //Explode
        }
    }
}
