using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int HP, SI, MI, LI, maxHealth;
    private float Row, Line;
    private Rigidbody rigi;
    //this script is a placeholder, as the real one is unavailable
    void Start()
    {
        HP = 70;
        SI = 20;
        MI = 40;
        LI = 60;
        maxHealth = 100;
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
        if (collide.gameObject.tag == "SmallHealthInstant")
        {
            if (HP <= (maxHealth - SI))
            {
                HP += SI;
            }
            else
            {
                HP = maxHealth;
            }
            Destroy(collide.gameObject);
        }
        else if (collide.gameObject.tag == "MediumHealthInstant")
        {
            if (HP <= (maxHealth - MI))
            {
                HP += MI;
            }
            else
            {
                HP = maxHealth;
            }
            Destroy(collide.gameObject);
        }
        else if (collide.gameObject.tag == "LargeHealthInstant")
        {
            if (HP <= (maxHealth - LI))
            {
                HP += LI;
            }
            else
            {
                HP = maxHealth;
            }
            Destroy(collide.gameObject);
        }

    }
}
