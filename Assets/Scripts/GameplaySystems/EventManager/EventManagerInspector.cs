using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventManager))]
//[CanEditMultipleObjects]
public class EventManagerInspector : Editor
{
	private Type[] _conditionImplementations, _eventImplementations;
	private int _implementationTypeIndex;

	public override void OnInspectorGUI()
	{
		EventManager eventManager = target as EventManager;
		//specify type
		if (eventManager == null)
		{
			return;
		}

		if (_conditionImplementations == null || GUILayout.Button("Refresh condition implementations"))
		{
			//this is probably the most imporant part:
			//find all implementations of INode using System.Reflection.Module
			_conditionImplementations = GetImplementations<Condition>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
		}
		EditorGUILayout.LabelField($"Found {_conditionImplementations.Count()} condition implementations");

		//select implementation from editor popup
		_implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Condition Type"),
									_implementationTypeIndex, _conditionImplementations.Select(impl => impl.FullName).ToArray());

		//for (int conditionsIndex = 0; conditionsIndex < eventManager.conditions.Count; conditionsIndex++)
		for (int actionsIndex = 0; actionsIndex < eventManager.actions.Count; actionsIndex++)
		{

			if (eventManager.actions[actionsIndex] == null)
			{
				eventManager.actions[actionsIndex] = new EventAction();
			}

			for (int conditionsIndex = 0;
				(eventManager.actions[actionsIndex] != null)
					&& (conditionsIndex < eventManager.actions[actionsIndex].conditions.Count);
				conditionsIndex++)
			{
				if (GUILayout.Button("Set type of Actions Element " + actionsIndex + ", Conditions Element " + conditionsIndex))
				{
					//set new value
					//eventManager.conditions[conditionsIndex] = (Condition)Activator.CreateInstance(_implementations[_implementationTypeIndex]);
					eventManager.actions[actionsIndex].conditions[conditionsIndex] = (Condition)Activator.CreateInstance(_conditionImplementations[_implementationTypeIndex]);
				}
			}
		}

		if (_eventImplementations == null || GUILayout.Button("Refresh condition implementations"))
		{
			//this is probably the most imporant part:
			//find all implementations of INode using System.Reflection.Module
			_eventImplementations = GetImplementations<Event>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
		}

		EditorGUILayout.LabelField($"Found {_eventImplementations.Count()} event implementations");

		//select implementation from editor popup
		_implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Event Type"),
									_implementationTypeIndex, _eventImplementations.Select(impl => impl.FullName).ToArray());

		//for (int conditionsIndex = 0; conditionsIndex < eventManager.conditions.Count; conditionsIndex++)
		for (int actionsIndex = 0; actionsIndex < eventManager.actions.Count; actionsIndex++)
		{

			if (eventManager.actions[actionsIndex] == null)
			{
				eventManager.actions[actionsIndex] = new EventAction();
			}

			for (int eventsIndex = 0;
				(eventManager.actions[actionsIndex] != null)
					&& (eventsIndex < eventManager.actions[actionsIndex].events.Count);
				eventsIndex++)
			{
				if (GUILayout.Button("Set type of Actions Element " + actionsIndex + ", Events Element " + eventsIndex))
				{
					//set new value
					//eventManager.conditions[conditionsIndex] = (Condition)Activator.CreateInstance(_implementations[_implementationTypeIndex]);
					eventManager.actions[actionsIndex].events[eventsIndex] = (Event)Activator.CreateInstance(_eventImplementations[_implementationTypeIndex]);
				}
			}
		}

		base.OnInspectorGUI();
	}

	private static Type[] GetImplementations<T>()
	{
		var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

		var interfaceType = typeof(T);
		return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
	}
}