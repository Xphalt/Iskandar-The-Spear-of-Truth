using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public SwitchManager manager;

    public Material offMat, onMat;
    private MeshRenderer myMesh;

    public GameObject spotLight;

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
        spotLight.SetActive(false);
        myMesh.material = offMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerSword") && !spotLight.activeSelf)
        {
            myMesh.material = onMat;
            spotLight.SetActive(true);
            manager.RegisterSwitch(this);
        }
    }
}
