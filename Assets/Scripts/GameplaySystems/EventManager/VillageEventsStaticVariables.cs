using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VillageEventsStaticVariables
{
    public enum VillageEventStages
    {
        villageStarted,
        forestComplete,
        desertComplete,
        darkLordDungeonComplete
    };

    public static bool villageHasStarted = true;
    public static bool desertIsCompleted;
    public static bool forestIsCompleted;
    public static bool darkLordDungeonIsCompleted;

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
            case VillageEventStages.forestComplete:
                forestIsCompleted = true;
                break;
            case VillageEventStages.darkLordDungeonComplete:
                darkLordDungeonIsCompleted = true;
                break;
            default:
                break;
        }
    }

    private static void DisableAllEventTypes()
    {
        desertIsCompleted = false;
        forestIsCompleted = false;
        darkLordDungeonIsCompleted = false;
    }
}
