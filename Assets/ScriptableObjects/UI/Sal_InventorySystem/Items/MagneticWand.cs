using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticWand : MonoBehaviour
{
    public float beamSpeed;
    public float travelTime;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime < travelTime)
            transform.position += Vector3.forward * beamSpeed * Time.deltaTime; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Contains("MagneticObj"))
        {

        }
    }
}
