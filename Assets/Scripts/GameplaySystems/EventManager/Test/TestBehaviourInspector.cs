using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestBehaviour))]
//[CanEditMultipleObjects]
class TestBehaviourInspector : Editor
{
	private Type[] _implementations;
	private int _implementationTypeIndex;

	public override void OnInspectorGUI()
	{
		TestBehaviour testBehaviour = target as TestBehaviour;
		//specify type
		if (testBehaviour == null)
		{
			return;
		}

		if (_implementations == null || GUILayout.Button("Refresh implementations"))
		{
			//this is probably the most imporant part:
			//find all implementations of INode using System.Reflection.Module
			_implementations = GetImplementations<INode>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
		}


		EditorGUILayout.LabelField($"Found {_implementations.Count()} implementations");


		//select implementation from editor popup
		_implementationTypeIndex = EditorGUILayout.Popup(	new GUIContent("Implementation"),
															_implementationTypeIndex, _implementations.Select(impl => impl.FullName).ToArray());
		for (int i = 0; i < testBehaviour.nodes.Count; i++)
		{
			if (GUILayout.Button("Create instance " + i))
			{
				//set new value
				testBehaviour.nodes[i] = (INode)Activator.CreateInstance(_implementations[_implementationTypeIndex]);
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