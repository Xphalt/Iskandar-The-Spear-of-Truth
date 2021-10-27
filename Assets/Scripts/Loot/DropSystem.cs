using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    type1,
    type2,
    type3,
    Chest
}

public class DropSystem : MonoBehaviour
{
    private static DropSystem instance;
    public static DropSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DropSystem>();
            }
            return instance;
        }
    }

    [SerializeField]
    public List<EntityType> entityType = new List<EntityType>();
    [SerializeField]
    public List<int> tentatives = new List<int>(); 
    public Dictionary<EntityType, int> tentativeNum = new Dictionary<EntityType, int>();
     

    // Start is called before the first frame update
    void Awake()
    {   
        for (int i = 0; i < entityType.Count; i++)
        {
            tentativeNum.Add(entityType[i], tentatives[i]);
        } 
    }


    public float CalculateChance(EntityType type, float chance)
    {
        Debug.Log(string.Concat("Chance: ", 1.0f - (Mathf.Pow((1.0f - (chance / 100)), tentativeNum[type]))));
        return 1.0f - (Mathf.Pow((1.0f - (chance/100.0f)), tentativeNum[type]));
    }

    public void ResetTentativeNum(EntityType type)
    {
        tentativeNum[type] = 1;
    }

    public void IncreaseTentatives(EntityType type)
    {
        ++tentativeNum[type];
    }
}
