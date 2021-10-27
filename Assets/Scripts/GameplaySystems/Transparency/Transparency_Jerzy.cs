using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency_Jerzy : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        if (!player) player = FindObjectOfType<PlayerMovement_Jerzy>().transform;
    }

    void Update()
    {
        Vector3 fromPosition = transform.position;
        Vector3 toPosition = player.position;
        Vector3 direction = toPosition - fromPosition;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, Vector3.Distance(player.position, transform.position));

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                hit.collider.gameObject.GetComponent<TransparentObject_Jerzy>().MakeTransparent(i+1);
            }
        }
    }
}
