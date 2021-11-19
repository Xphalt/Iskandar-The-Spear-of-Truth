using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, ISerializationCallbackReceiver
{
    public float Damage; 
    public float explosionRadius;
    private float currentTime;
    public float timeBeforeDetonating;

    private SphereCollider sphereCollider;
    public GameObject explosionEffect;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeBeforeDetonating)
        {
            var effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 10.0f);
            Destroy(gameObject, 1.0f);
        }
    }

    public void OnBeforeSerialize()
    {
        if (sphereCollider == null) sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = explosionRadius;
    }

    public void OnAfterDeserialize()
    { }

    private void OnTriggerStay(Collider other)
    {
        //Gets the right componento to deal damage
        StatsInterface stats = null; 
        other.TryGetComponent<StatsInterface>(out stats); 
         
        //takes damage after detonation  
        if (currentTime > timeBeforeDetonating)
        {
            if(stats) //Deal damage
                stats.DealDamage(stats, Damage); 

            //Destroy rock
            if(other.gameObject.tag.Contains("BombPuzzle"))
                Destroy(other.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Destroy rocks after detonation  
        if (currentTime > timeBeforeDetonating)
        { 
            if (collision.gameObject.tag.Contains("BombPuzzle"))
                Destroy(collision.gameObject);
        }
    }
}
