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

[System.Serializable]
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

public class EventsCompleted : Condition
{
	public override bool TestCondition()
	{
		return EventManager.actions[ActionIndex].events[EventIndex].IsComplete;
	}

	[SerializeField] EventManager EventManager;
	[SerializeField] int ActionIndex;
	[SerializeField] int EventIndex;
}

[System.Serializable]
public class ConditionCompleted : Condition
{
	public override bool TestCondition()
	{
		return EventManager.actions[ActionIndex].conditions[conditionIndex].TestCondition();
	}

	[SerializeField] EventManager EventManager;
	[SerializeField] int ActionIndex;
	[SerializeField] int conditionIndex;
}

public class CraftArmourQuestCondition : Condition
{
	//Check if player has item and if has enough
    public override bool TestCondition()
    {
        for (int i = 0; i < materials.Length; i++)
        {
			InventorySlot slot = playerInventory.FindItemOnInventory(materials[i].data);
			if (slot != null)
			{
				if (slot.amount < amounts[i])
				{
					return false;
				}
				else if (slot.amount >= amounts[i])
				{
					continue;
				}
			}
			else
				return false;
        } 

		return true;
    }

	[SerializeField] InventoryObject_Sal playerInventory;
	[SerializeField] ItemObject_Sal[] materials;
	[SerializeField] int[] amounts;
}

public class CraftArmourDialogueCondition : Condition
{
    public override bool TestCondition()
    {
		if(condToComplete.TestCondition() && dialogueEnded.TestCondition()) //Quest completed and dialogue with blacksmit is ended
        { 
			return true;
        }
		return false;
    }

	[SerializeField] ConditionCompleted condToComplete; 
	[SerializeField] DialogueEndedCondition dialogueEnded;  
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