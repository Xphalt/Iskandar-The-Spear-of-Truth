using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public abstract class Condition
{
	public abstract bool TestCondition();

	[SerializeReference] public double testDouble;
	[SerializeReference] public int testInt;
}

[System.Serializable]
public class KillCondition : Condition
{
	public override bool TestCondition()
	{
		return true;
	}

	[SerializeReference] public List<GameObject> enemiesToKill;

	[SerializeReference] public int childTestInt;
	[SerializeReference] public float childTestFloat;

	[SerializeReference] public bool childTestBool;
}
