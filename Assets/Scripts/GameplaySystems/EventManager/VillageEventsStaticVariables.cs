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

    public static bool villageHasStarted = true;
    public static bool desertIsCompleted;
    public static bool forestDungeonIsCompleted;
    public static bool desertDungeonIsCompleted;

    public static void UpdateVillage(VillageEventStages eventStage)
    {
        DisableAllEventTypes();

        switch (eventStage)
        {
            case VillageEventStages.villageStarted:
                villageHasStarted = true;
                break;
            case VillageEventStages.desertComplete:
                desertIsCompleted = true;
                break;
            case VillageEventStages.forestDungeonComplete:
                forestDungeonIsCompleted = true;
                break;
            case VillageEventStages.desertDungeonComplete:
                desertDungeonIsCompleted = true;
                break;
            default:
                break;
        }
    }

    private static void DisableAllEventTypes()
    {
        desertIsCompleted = false;
        forestDungeonIsCompleted = false;
        desertDungeonIsCompleted = false;
    }
}
