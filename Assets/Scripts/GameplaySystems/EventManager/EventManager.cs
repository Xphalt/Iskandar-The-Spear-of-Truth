using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition
{
    public virtual bool TestCondition()
    {
        return true;
	}
}

[System.Serializable]
public class Event
{
    public virtual void TriggerEvent()
    {
        // do stuff here
	}
}

[System.Serializable]
public class KillCondition : Condition
{
	public override bool TestCondition()
	{
		return base.TestCondition();
	}

    public bool testBool;
}



public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public struct Action
    {
        public List<Condition> conditions;
        public List<Event> events;
    }

    [SerializeField] private List<Action> actions;  
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
