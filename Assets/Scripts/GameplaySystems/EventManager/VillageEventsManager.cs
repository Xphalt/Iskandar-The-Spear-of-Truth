using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageEventsManager : MonoBehaviour
{
    [SerializeField] private GameObject introVillageEvents;
    [SerializeField] private GameObject desertConnectionEvents;
    [SerializeField] private GameObject forestConnectionEvents;
    [SerializeField] private GameObject darkLordDungeonConnectionEvents;

    public void SetEvents()
    {
        introVillageEvents.SetActive(false);
        desertConnectionEvents.SetActive(false);
        forestConnectionEvents.SetActive(false);
        darkLordDungeonConnectionEvents.SetActive(false);

        // Do in reverse order so the latest events are enabled
        if (VillageEventsStaticVariables.levelsComplete[(int)VillageEventsStaticVariables.VillageEventStages.desertDungeonComplete])
            darkLordDungeonConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.levelsComplete[(int)VillageEventsStaticVariables.VillageEventStages.desertComplete])
            desertConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.levelsComplete[(int)VillageEventsStaticVariables.VillageEventStages.forestDungeonComplete])
            forestConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.levelsComplete[(int)VillageEventsStaticVariables.VillageEventStages.villageStarted])
            introVillageEvents.SetActive(true);
    }
}
