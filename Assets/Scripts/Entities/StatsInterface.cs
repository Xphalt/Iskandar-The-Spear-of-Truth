using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatsInterface : MonoBehaviour
{
    public float health;
    protected SoundPlayer sfx;

    public abstract void TakeDamage(float amount);
    public abstract void DealDamage(StatsInterface target,float amount);
}
