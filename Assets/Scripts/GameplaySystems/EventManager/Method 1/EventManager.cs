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
}

public class EventManager : MonoBehaviour
{
    [SerializeReference] public List<EventAction> actions = new List<EventAction>();
    
    //morgan's smelly edit 
    public List<EventAction> totallynotcompletedevents = new List<EventAction>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //mog>
        totallynotcompletedevents.Clear();
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].complete)
            {
                totallynotcompletedevents.Add(actions[i]);
            }
        }
        //<mog
        foreach (EventAction action in actions)
        { 
            if (!action.complete)
            {
                // Test conditions are met
                //action.complete = true; // mog edit (that jusxtaposes the save system, bar this it all works fine!)
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

    }

    //morgan save edit
    public EventAction getCompleted(int x)
    {
        return actions[x];
    }

    public void setCompleted(int x, bool IsCompleted)
    {
        actions[x].complete = IsCompleted;
    }

    public bool getcompletionbool(int x)
    {
        return actions[x].complete; 
    }

    public int getamountofevents()
    {
        return actions.Count();
    }


    public void RandomiseEvents()
    {
        string output = "";
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].complete = (UnityEngine.Random.Range(0, 2) == 0 ? false : true);
            if (actions[i].complete)
            {
                output += ",T";
            }
            else
            {
                output += ",F";
            }
        }
    }
}