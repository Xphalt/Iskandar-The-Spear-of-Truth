using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VillageEventsStaticVariables
{
    public enum VillageEventStages
    {
        villageStarted,
        forestDungeonComplete,
        desertComplete,
        desertDungeonComplete,
    };

    public static bool[] levelsComplete = { true, false, false, false };

    public static void UpdateVillage(VillageEventStages eventStage)
    {
        DisableAllEventTypes();

        levelsComplete[(int)eventStage] = true;
    }

    private static void DisableAllEventTypes()
    {
        for (int l = 0; l < levelsComplete.Length; l++)
        {
            if (l != (int)VillageEventStages.villageStarted)
                levelsComplete[l] = false;
        }
    }
}
