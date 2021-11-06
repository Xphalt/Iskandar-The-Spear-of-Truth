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


    void Update()
    {
        currentTime += Time.deltaTime; 
        if (currentTime > (timeBeforeDetonating + 1.0f))
            Destroy(gameObject);
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
        if (stats && currentTime > timeBeforeDetonating)
            stats.DealDamage(stats, Damage); 
    }
}
