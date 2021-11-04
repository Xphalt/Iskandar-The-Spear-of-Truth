using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class EntityDrop : MonoBehaviour
{
    private float spawnSpeed = 150.0f;

    public float disableDelay = 0;

    [Range(1, 100)] public List<float> dropChances;
    public List<ItemObject_Sal> itemsToDrop;
    private List<bool> chances;
    public EntityType type;

    public GameObject groundItem;
    
    private Vector3 randomSpawnDir;
    
    public void SpawnLoot()
    {
        CalculateDropProbability();

        Vector3 playerToObjDir = transform.position - GameObject.FindObjectOfType<PlayerStats>().transform.position;

        bool canIReset = false;
        for (int i = 0; i < chances.Count; i++)
        {
            if (chances[i])
            {
                do
                {
                    float x = Rnd.Range(-1.0f, 1.0f);
                    float z = Rnd.Range(-1.0f, 1.0f);
                    randomSpawnDir = new Vector3(x, 0.0f, z).normalized;
                } while (Vector3.Dot(randomSpawnDir, playerToObjDir) < 0); //dirAngle > 180 && dirAngle < 180

                //Spawn 
                GameObject obj = Instantiate(groundItem, transform.position, Quaternion.identity);
                //Set Item
                obj.GetComponent<GroundItem>().SetItem(itemsToDrop[i]);
                //Set spawn type
                if (type == EntityType.Chest)
                    obj.GetComponent<GroundItem>().spawn += ChestSpawn;
                else
                    obj.GetComponent<GroundItem>().spawn += NormalSpawn;

                canIReset = true;
            }
        }

        if(canIReset)
            //Reset tentatives to 1
            DropSystem.Instance.ResetTentativeNum(type);
        else
            DropSystem.Instance.IncreaseTentatives(type);

        //Destroy
        if (type != EntityType.Chest)
            Destroy(gameObject, disableDelay); //GOING TO HANDLE THIS IN OTHER SCRIPTS
    }

    public void CalculateDropProbability()
    {
        chances = new List<bool>();
        for (int i = 0; i < dropChances.Count; i++)
        {
            float randomChance = Rnd.Range(0.0f, 1.0f);
            float dropChance = DropSystem.Instance.CalculateChance(type, this.dropChances[i]);
            Debug.Log($"Random number: {randomChance}");
            if (randomChance < dropChance)// && Mathf.Abs(randomChance - dropChance) >= .02f)
                chances.Add(true);
            else
                chances.Add(false);
        }
    }

    //Function delegates 
    public void NormalSpawn(GameObject obj)
    {
        //Set dir
        obj.GetComponent<Rigidbody>().velocity = randomSpawnDir * spawnSpeed * Time.deltaTime;
    }

    public void ChestSpawn(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().velocity = Vector3.up * Time.deltaTime; 
    }
}
