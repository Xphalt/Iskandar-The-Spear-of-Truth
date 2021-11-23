using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public SwitchManager manager;

    public Material offMat, onMat;
    private MeshRenderer myMesh;

    public GameObject spotLight;

    public bool startLight = false;
    private bool on = false;

    //private bool activated = false;

    private void Start()
    {
        if (!manager) manager = FindObjectOfType<SwitchManager>();
        if (!manager) enabled = false;

        myMesh = GetComponent<MeshRenderer>();
        TurnOff();
    }

    public void TurnOff()
    {
        spotLight.SetActive(startLight);
        myMesh.material = offMat;
        on = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerSword") && !on)
        {
            myMesh.material = onMat;
            spotLight.SetActive(!startLight);
            on = true;
            manager.RegisterSwitch(this);
        }
    }
}
