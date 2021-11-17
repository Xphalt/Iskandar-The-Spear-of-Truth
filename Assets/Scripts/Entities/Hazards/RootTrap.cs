using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrap : MonoBehaviour
{
    public float rootDuration;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats stats))
        {
            if (!stats.poisonProtection)
            {
                other.gameObject.GetComponent<PlayerMovement_Jerzy>().Root(rootDuration);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            {
                anim.Play("EnviromentalHazardsRoot");
                other.gameObject.GetComponent<PlayerMovement_Jerzy>().Root(rootDuration);
            }
        }
    }
}
