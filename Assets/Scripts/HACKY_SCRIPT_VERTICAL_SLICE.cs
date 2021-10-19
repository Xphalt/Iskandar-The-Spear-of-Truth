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
    public bool spawnedLittleOrcs = false;
    public bool spawnedBigOrc = false;

    private void Awake()
    {
        instance = this;
        foreach (GameObject lo in little_orcs) lo.SetActive(false);
    }

    private void Update()
    {
        if (hasSpokenToMum)
        {
            milkperson.SetActive(true);
            
        }

        if (hasSpokenToMilkperson && !spawnedLittleOrcs)
        {
            for (int i = 0; i < little_orcs.Length; i++)
            {
                little_orcs[i].SetActive(true);
            }
            spawnedLittleOrcs = true;
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

        if (hasDefeatedLittleOrcs && !spawnedBigOrc)
        {
            big_orc.SetActive(true);
            spawnedBigOrc = true;
        }
    }
}
