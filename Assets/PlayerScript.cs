using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int HP;
    private float Row, Line;
    private Rigidbody rigi;
    //this script is a placeholder, as the real one is unavailable
    void Start()
    {
        HP = 70;
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {        
        Row = Input.GetAxis("Horizontal");
        Line = Input.GetAxis("Vertical");
        Vector3 MoveVec = new Vector3(Row, 0, Line);

        if (MoveVec.magnitude > 1)
        {
            MoveVec = MoveVec.normalized;
        }

        rigi.velocity = MoveVec * 2;
    }

    void OnCollisionEnter(Collision collide)
    {
        if (collide.collider.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            Physics.IgnoreCollision(collide.transform.GetComponent<Collider>(), GetComponent<Collider>());
        }
        if (collide.gameObject.tag == "HealthInstant")
        {
            if (HP <= 80)
            {
                HP += 20;
            }
            else
            {
                HP = 100;
            }
            Destroy(collide.gameObject);
        }
        
    }
}
