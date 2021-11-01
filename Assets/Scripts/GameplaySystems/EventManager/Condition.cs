using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public abstract class Condition
{
	//public void OnEnable()
	//{
	//	hideFlags = HideFlags.HideAndDontSave;
	//}

	//public abstract void OnGUI();



	public abstract bool TestCondition();

	[SerializeReference] public double testDouble;
	[SerializeReference] public int testInt;
}

//[CreateAssetMenu(fileName = "New KillCondition", menuName = "EventManager/Conditions/KillCondition")]
[System.Serializable]
public class KillCondition : Condition
{
	//public override void OnGUI()
	//{
	//	childTestBool = EditorGUILayout.Toggle("BoolField", childTestBool);
	//	childTestInt = EditorGUILayout.IntField("IntField", childTestInt);
	//	childTestFloat = EditorGUILayout.FloatField("FloatField", childTestFloat);
	//}

	public override bool TestCondition()
	{
		return true;
	}

	[SerializeReference] public List<GameObject> enemiesToKill;

	[SerializeReference] public int childTestInt;
	[SerializeReference] public float childTestFloat;

	[SerializeReference] public bool childTestBool;
}
