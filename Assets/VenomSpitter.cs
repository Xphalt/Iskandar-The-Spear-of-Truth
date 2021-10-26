using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpitter : MonoBehaviour
{
    public float shootDelay;
    public float venomSpeed;
    public GameObject VenomBulletPrefab;
    private float timeSinceLastShot;

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && timeSinceLastShot >= shootDelay)
        {
            timeSinceLastShot = 0;
            GameObject venomShot = Instantiate(VenomBulletPrefab, transform.position, Quaternion.identity);
            venomShot.GetComponent<VenomShot>().SetDirection((other.gameObject.transform.position- transform.position).normalized,venomSpeed);
        }
    }
}
