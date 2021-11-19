using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceSwitch : MonoBehaviour
{
    public Animator fenceAnim;
    private int openfenceHash = Animator.StringToHash("isOpen");
    private bool isOpen = false;
    
    void Update()
    {
        fenceAnim.SetBool(openfenceHash, isOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("MagneticObj"))
            isOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("MagneticObj"))
            isOpen = false;
    }
}
