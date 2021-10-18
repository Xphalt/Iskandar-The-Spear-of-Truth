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

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= shootingRange && canShoot)
        {
            canShoot = false;
            Shoot();
        }

        if (canShoot == false)
        {
            transform.LookAt(player.transform.position, Vector3.up);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                canShoot = true;
                timer = attackDuration;
                transform.LookAt(player.transform.position, Vector3.up);
            }
        }
        
       
    }

    Vector3 CalculateVelocity(Vector3 startPos, Vector3 target, float time)
    {
        //find distance x and y first
        Vector3 distance = startPos - target;
        
        //find distance on x and z axis
        Vector3 distance_x_z = distance;
        distance_x_z.y = 0;

        //creating a float for the vertical height
        float projectileHeight = distance.y;
        float DistanceOnX_Z = distance_x_z.magnitude;

        //calculating initial x velocity
        //velocityX = x / t
        float velocityX_Z = DistanceOnX_Z / time;

        //calculating initial y velocity
        //velocityY = (y/t) + 1/2 * g * t
        float velocityY = projectileHeight / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distance_x_z.normalized;
        result *= velocityX_Z;
        result.y = velocityY;

        return result;
    }

    public void Shoot()
    {
        Vector3 projectileVelocity = CalculateVelocity(player.transform.position, transform.position, attackDuration);
        GameObject projectile = Instantiate(projectilePrefab, shootpoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = projectileVelocity;
    }
}
