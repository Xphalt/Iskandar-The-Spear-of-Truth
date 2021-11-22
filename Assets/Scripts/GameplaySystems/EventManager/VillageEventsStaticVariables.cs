using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VillageEventsStaticVariables
{
    public enum VillageEventStages
    {
        villageComplete,
        desertComplete,
        forestComplete,
        darkLordDungeonComplete   
    };

    public static bool villageIsCompleted;
    public static bool desertIsCompleted;
    public static bool forestIsCompleted;
    public static bool darkLordDungeonIsCompleted;

    public static void UpdateVillage(VillageEventStages eventStage)
    {
        DisableAllEventTypes();

        switch (eventStage)
        {
            case VillageEventStages.villageComplete:
                villageIsCompleted = true;
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
        villageIsCompleted = false;
        desertIsCompleted = false;
        forestIsCompleted = false;
        darkLordDungeonIsCompleted = false;
    }
}
