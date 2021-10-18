using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HACKY_SCRIPT_VERTICAL_SLICE : MonoBehaviour
{
    public GameObject[] little_orcs;
    public GameObject big_orc;
    public GameObject milkperson;

    public static HACKY_SCRIPT_VERTICAL_SLICE instance;
    public bool hasSpokenToMum = false;
    public bool hasSpokenToMilkperson = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (hasSpokenToMum)
        {
            milkperson.SetActive(true);
            
        }

        if (hasSpokenToMilkperson)
        {
            for (int i = 0; i < little_orcs.Length; i++)
            {
                little_orcs[i].SetActive(true);
            }
        }

        bool hasDefeatedLittleOrcs = true;
        for (int i = 0; i < little_orcs.Length; i++)
        {
            if (!little_orcs[i].GetComponent<EnemyStats>().HasBeenDefeated())
            {
                hasDefeatedLittleOrcs = false;
                break;
            }
        }

        if (hasDefeatedLittleOrcs)
        {
            big_orc.SetActive(true);
        }
    }
}
