using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class EventAction
{
    [SerializeReference]    public List<Condition> conditions = new List<Condition>();
    [SerializeReference]    public List<Event> events = new List<Event>();
    [SerializeField]        public bool complete = false;   
    [SerializeField]        public bool singleCondition = false;   
}

public class EventManager : MonoBehaviour
{
    [SerializeReference] public List<EventAction> actions = new List<EventAction>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (EventAction action in actions)
        {
            if(!action.singleCondition) //AND
            {
                if (!action.complete)
                {
                    // Test conditions are met
                    action.complete = true;
                    foreach (Condition condition in action.conditions)
                    {
                        if (!condition.TestCondition())
                        {
                            action.complete = false;
                            break;
                        }
                    }

                    // If all conditions are met execute the connected events
                    if (action.complete)
                    {
                        foreach (Event actionEvent in action.events)
                        {
                            actionEvent.TriggerEvent();
                        }
                    }
                }
            }
            else //OR
            {
                if (!action.complete)
                {
                    action.complete = true;
                    // Test conditions are met
                    foreach (Condition condition in action.conditions)
                    {
                        action.complete = false;
                        if (condition.TestCondition())
                        {
                            //Pass data to the event 
                            foreach (Event actionEvent in action.events)
                            {
                                actionEvent.PassDataFromCondition(condition.dataToPass);
                                actionEvent.TriggerEvent();
                            } 
                            return;
                        }
                    }
                }
            }
        }
    }
}
