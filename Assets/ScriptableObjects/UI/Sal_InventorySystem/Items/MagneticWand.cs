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
        if (currentTime < travelTime)
            transform.Translate(Vector3.forward * beamSpeed * Time.deltaTime, Space.Self);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Contains("MagneticObj"))
        {
            var magneticObj = other.GetComponent<MagneticObj>();

            //Set camera initial pos and rot
            magneticObj.initialPos = Camera.main.transform.position;
            magneticObj.initialRot = Camera.main.transform.rotation;

            //Set lerping state
            magneticObj.IsLerping = true;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Destroy(gameObject);
        }
    }
}
