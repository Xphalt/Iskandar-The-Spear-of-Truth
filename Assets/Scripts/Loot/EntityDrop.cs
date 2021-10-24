using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class EntityDrop : MonoBehaviour
{
    private float spawnSpeed = 150.0f;

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
            obj.GetComponent<GroundItem>().OnBeforeSerialize(); //Doesn't get called automatically for some reason
            //Set dir
            obj.GetComponentInChildren<Rigidbody>().velocity = randomSpawnDir * spawnSpeed * Time.deltaTime;

            DropSystem.Instance.ResetTentativeNum(type);
            //Destroy
            StartCoroutine(StopForceAndDestroy(obj));
        }
        else
        {
            DropSystem.Instance.IncreaseTentatives(type);
        }
    }

    IEnumerator StopForceAndDestroy(GameObject obj)
    {      
        yield return new WaitForSeconds(1.5f);
        //Stop drop
        Rigidbody objRgdBody = obj.GetComponentInChildren<Rigidbody>();
        objRgdBody.velocity = Vector3.zero;
        objRgdBody.angularVelocity = Vector3.zero;

        Destroy(gameObject);
    }
}
