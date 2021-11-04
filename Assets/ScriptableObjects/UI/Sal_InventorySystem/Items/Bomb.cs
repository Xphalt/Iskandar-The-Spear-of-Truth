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
        StatsInterface statsPlayer = null;
        StatsInterface statsEnemy = null;
        try
        {
            statsPlayer = other.GetComponent<PlayerStats>();
        }
        catch
        { }
        try
        {
            statsEnemy = other.GetComponent<EnemyStats>();
        }
        catch { }
         
        //takes damage after detonation 

        if (statsPlayer && currentTime > timeBeforeDetonating)
            statsPlayer.DealDamage(statsPlayer, Damage);
        if (statsEnemy && currentTime > timeBeforeDetonating)
            statsEnemy.DealDamage(statsEnemy, Damage);
    }
}
