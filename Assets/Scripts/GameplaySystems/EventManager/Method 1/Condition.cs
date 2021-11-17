using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public abstract class Condition
{
	public abstract bool TestCondition(); 
}


[System.Serializable]
// Checks that the active state of all the objects in the list equate to checkObjectsAreActive.
// If you want to check all the objects are disabled, set checkObjectsAreActive to false.
// Can be used to check if a group of enemies is dead, item(s) have been collected etc.
public class ObjectsActiveCondition : Condition
{
	public override bool TestCondition()
	{
		foreach(GameObject gameObject in _objectsToCheck)
		{
			if(gameObject.activeSelf != checkObjectsAreActive)
			{
				return false;
			}
		}

		return true;
	}

	[SerializeReference] private List<GameObject> _objectsToCheck;
	[SerializeReference] private bool checkObjectsAreActive = false;
}

//[System.Serializable]
//public class ItemCollectionCondition : Condition
//{
//	public override bool TestCondition()
//	{
//		return false;
//	}
//}

[System.Serializable]
public class TriggerColliderCondition : Condition
{
	public override bool TestCondition()
	{
		if(_oneTimeTrigger.HasTriggered)
		{
			return true;
		}
		return false;
	}

	[SerializeField] private OneTimeTrigger _oneTimeTrigger;
}

public class IsDeadCondition : Condition
{
    public override bool TestCondition()
    {
		if (playerStats.health <= 0)
			return true;
		return false;
    }

	[SerializeField] private PlayerStats playerStats;
}

public class EnemyDiedCondition : Condition
{
	public override bool TestCondition()
	{
		if (enemyStats.IsDead())
			return true;
		return false;
	}

	[SerializeField] private EnemyStats enemyStats;
}

public class DialogueEndedCondition : Condition
{
    public override bool TestCondition()
    {
		if (dlgManager.NewConversation == dialogue && dlgManager.ConversationIsEnded)
			return true;
		return false;
    }

	[SerializeField] private NewConversation dialogue;
	[SerializeField] private DialogueManager dlgManager;
}

public class KillnEnemiesCondition : Condition
{
	public override bool TestCondition()
	{
		if (EnemyStats.EnemiesKilled >= amountToKill)
			return true;
		return false;
	}

	[SerializeField] private int amountToKill;
}

//[System.Serializable]
//public class DialogueCondition : Condition
//{
//	public override bool TestCondition()
//	{
//		return false;
//	}
//}

//[System.Serializable]
//public class MenuOpenCondition : Condition
//{
//	public override bool TestCondition()
//	{
//		return false;
//	}
//}