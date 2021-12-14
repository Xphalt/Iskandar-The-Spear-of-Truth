using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public GameObject Health;
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Health.transform.position, GameObject.Find("Player").transform.position);

        if (dist < 13.5)
        {
            Health.gameObject.SetActive(true);
        }
        else
        {
            Health.gameObject.SetActive(false);
        }
    }
}
