using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageEventsManager : MonoBehaviour
{
    [SerializeField] private GameObject introVillageEvents;
    [SerializeField] private GameObject desertConnectionEvents;
    [SerializeField] private GameObject forestConnectionEvents;
    [SerializeField] private GameObject darkLordDungeonConnectionEvents;

    private List<GameObject> events = new List<GameObject>();

    void Start()
    {
        events.Add(introVillageEvents);
        events.Add(desertConnectionEvents);
        events.Add(forestConnectionEvents);
        events.Add(darkLordDungeonConnectionEvents);

        foreach (GameObject eventType in events)
            eventType.SetActive(false);
    }
}
