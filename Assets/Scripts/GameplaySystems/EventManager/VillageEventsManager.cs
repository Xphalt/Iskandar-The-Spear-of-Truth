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

        if (VillageEventsStaticVariables.villageIsCompleted)
            introVillageEvents.SetActive(true);
        else if (VillageEventsStaticVariables.villageIsCompleted)
            desertConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.villageIsCompleted)
            forestConnectionEvents.SetActive(true);
        else if (VillageEventsStaticVariables.villageIsCompleted)
            darkLordDungeonConnectionEvents.SetActive(true);
    }
}
