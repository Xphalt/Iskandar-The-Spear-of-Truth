using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class EntityDrop : MonoBehaviour
{
    private float spawnSpeed = 150.0f;

    //public float disableDelay = 0;

    [Range(1, 100)] public float dropChance;
    public ItemObject_Sal itemToDrop;
    public EntityType type;

    public GameObject groundItem;
    
    private Vector3 randomSpawnDir;

    public bool CalculateDropProbability()
    { 
        float randomChance = Rnd.Range(0.0f, 1.0f);
        float dropChance = DropSystem.Instance.CalculateChance(type, this.dropChance);
        Debug.Log($"Random number: {randomChance}");
        if (randomChance < dropChance)// && Mathf.Abs(randomChance - dropChance) >= .02f)
            return true;

        return false;
    }

    public void SpawnLoot()
    {
        if(CalculateDropProbability())
        {
            Vector3 playerToObjDir = transform.position - GameObject.FindObjectOfType<PlayerStats>().transform.position;
             
            do
            {
                float x = Rnd.Range(-1.0f, 1.0f);
                float z = Rnd.Range(-1.0f, 1.0f);
                randomSpawnDir = new Vector3(x, 0.0f, z).normalized;
            } while (Vector3.Dot(randomSpawnDir, playerToObjDir) < 0); //dirAngle > 180 && dirAngle < 180

            //Spawn 
            GameObject obj = Instantiate(groundItem, transform.position, Quaternion.identity);
            //Set Item
            obj.GetComponent<GroundItem>().SetItem(itemToDrop);
            //Set spawn type
            if (type == EntityType.Chest)
                obj.GetComponent<GroundItem>().spawn += ChestSpawn;
            else
                obj.GetComponent<GroundItem>().spawn += NormalSpawn;
             
            //Reset tentatives to 1
            DropSystem.Instance.ResetTentativeNum(type);
            //Destroy
            //if (type != EntityType.Chest)
                //Destroy(gameObject, disableDelay); GOING TO HANDLE THIS IN OTHER SCRIPTS
        }
        else
        {
            DropSystem.Instance.IncreaseTentatives(type);
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
