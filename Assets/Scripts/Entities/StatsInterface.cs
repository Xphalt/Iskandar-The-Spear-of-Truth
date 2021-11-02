using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatsInterface : MonoBehaviour
{
    public bool HasBeenDefeated => health <= 0;

    public float MAX_HEALTH = 10.0f;
    public float health;
    protected SoundPlayer sfx;

    public abstract void TakeDamage(float amount, bool scriptedKill = false);
    public abstract void DealDamage(StatsInterface target,float amount, bool scriptedKill = false);
}
