using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOpenFence : MonoBehaviour
{
    public Animator fenceAnim;
    private int openFenceHash = Animator.StringToHash("Open"); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Contains("playerSword"))
            fenceAnim.SetTrigger(openFenceHash);
    }
}
