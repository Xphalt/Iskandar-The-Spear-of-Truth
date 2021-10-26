using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyTest : MonoBehaviour
{
    [Header("Test Transforms")]
    public GameObject player;
    public GameObject projectilePrefab;
    public Transform shootpoint;

    [Header("Attack Variables")]
    public float attackDuration;
    public float shootingRange;
    private float timer;
    private float distance;
    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        timer = attackDuration;
    }

    // Update is called once per frame
    void Update()
    {



        //if (canShoot == false)
        //{
        //    transform.LookAt(player.transform.position, Vector3.up);
        //    timer -= Time.deltaTime;
        //    if (timer <= 0)
        //    {
        //        canShoot = true;
        //        timer = attackDuration;
        //        transform.LookAt(player.transform.position, Vector3.up);
        //    }
        //}
        
       
    }


}
