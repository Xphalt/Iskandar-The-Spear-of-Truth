using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public SwitchManager manager;

    private Material myMat;

    public Color activatedCol, deactivatedCol;

    //private bool activated = false;

    private void Start()
    {
        if (!manager) manager = FindObjectOfType<SwitchManager>();
        if (!manager) enabled = false;

        myMat = GetComponent<MeshRenderer>().material;
        myMat.color = deactivatedCol;
    }

    public void TurnOff()
    {
        myMat.color = deactivatedCol;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerSword"))
        {
            manager.RegisterSwitch(this);
            myMat.color = activatedCol;
        }
    }
}
