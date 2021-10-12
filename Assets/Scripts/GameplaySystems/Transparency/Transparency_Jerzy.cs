using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency_Jerzy : MonoBehaviour
{

    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 fromPosition = transform.position;
        Vector3 toPosition = player.transform.position;
        Vector3 direction = toPosition - fromPosition;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, Vector3.Distance(player.transform.position, transform.position));


        for (float i = 0; i < hits.Length; i+=1)
        {
            RaycastHit hit = hits[(int)i];

            if (hit.collider.gameObject.tag == "Wall")
            {

                hit.collider.gameObject.GetComponent<TransparentObject_Jerzy>().MakeTransparent(i+1);
            }

        }

    }
}
