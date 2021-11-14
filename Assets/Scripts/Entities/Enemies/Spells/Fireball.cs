using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExtraFunctions;

public class Fireball : ArcProjectile
{
    private Vector3 indicatorScale;

    private float damage, radius, knockback, knockbackDuration;

    public Transform indicator;

    public bool destroyOnEnd = true, growIndicator = false;

    public override void Start()
    {
        base.Start();
        rotVec = RandomVector3();
    }

    public override void TraverseArc()
    {
        base.TraverseArc();
        if (growIndicator) indicator.localScale = Vector3.Lerp(indicator.localScale, indicatorScale, timer / duration);
    }

    public override void EndArc()
    {
        base.EndArc();
        if (destroyOnEnd) Impact(true);
    }

    public void Launch(Vector3 _start, Vector3 _end, float _speed, float _damage, float _radius, float _knockback = 0, float _kbDuration = 0)
    {
        base.StartArc(_start, _end, _speed);

        damage = _damage;
        radius = _radius;
        knockback = _knockback;
        knockbackDuration = _kbDuration;

        indicator.position = end;
        indicator.SetParent(null);

        if (growIndicator)
        {
            indicator.localScale = new Vector3(0, indicator.localScale.y, 0);
            indicatorScale = new Vector3(radius * 2, indicator.localScale.y, radius * 2);
        }
        else indicator.localScale = new Vector3(radius * 2, indicator.localScale.y, radius * 2);

        indicator.gameObject.SetActive(true);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats player))
        {
            player.TakeDamage(damage);
            player.GetComponent<PlayerMovement_Jerzy>().KnockBack(transform.position, knockback, knockbackDuration);
            Impact();
        }
    }

    private void Impact(bool dealDamage = false)
    {
        if (dealDamage)
            foreach (Collider c in Physics.OverlapSphere(transform.position, radius)) OnTriggerEnter(c);
        
        FinalBurst explosion = transform.GetComponentInChildren<FinalBurst>(true);
        if (explosion) explosion.Burst();

        indicator.gameObject.SetActive(false);
    }
}
