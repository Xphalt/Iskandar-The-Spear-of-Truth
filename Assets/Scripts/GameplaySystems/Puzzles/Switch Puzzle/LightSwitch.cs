using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public SwitchManager manager;

    private Material myMat;

    public Color[] colourCycle;

    public int colourIndex = 0;

    //private bool activated = false;

    private void Start()
    {
        if (!manager) manager = FindObjectOfType<SwitchManager>();
        if (!manager) enabled = false;

        myMat = GetComponent<MeshRenderer>().material;
        colourIndex = 0;
        myMat.color = colourCycle[0];
    }

    public void TurnOff()
    {
        colourIndex = 0;
        SetColour();
    }

    private void SetColour()
    {
        myMat.color = colourCycle[colourIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerSword") && colourIndex + 1 < colourCycle.Length)
        {
            colourIndex++;
            SetColour();
            manager.RegisterSwitch(this);
        }
    }
}
