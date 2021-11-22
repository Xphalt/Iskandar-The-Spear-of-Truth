using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageEventsManager : MonoBehaviour
{
    [SerializeField] private GameObject introVillageEvents;
    [SerializeField] private GameObject desertConnectionEvents;
    [SerializeField] private GameObject forestConnectionEvents;
    [SerializeField] private GameObject darkLordDungeonConnectionEvents;

    void Start()
    {
        introVillageEvents.SetActive(false);
        desertConnectionEvents.SetActive(false);
        forestConnectionEvents.SetActive(false);
        darkLordDungeonConnectionEvents.SetActive(false);

        // Do in reverse order so the latest events are enabled
        if (VillageEventsStaticVariables.darkLordDungeonIsCompleted)
            darkLordDungeonConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.desertIsCompleted)
            desertConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.forestIsCompleted)
            forestConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.villageHasStarted)
            introVillageEvents.SetActive(true);
    }
}
