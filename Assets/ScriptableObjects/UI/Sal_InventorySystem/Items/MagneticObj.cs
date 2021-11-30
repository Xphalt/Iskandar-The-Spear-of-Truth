using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagneticObj : MonoBehaviour
{
    private bool isControllable;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    private Vector3 newDir;

    public Transform cameraPoint; 
    public float movementSpeed;

    //Lerp stuff
    private bool isLerping;
    public bool IsLerping
    {
        get { return isLerping; }
        set { isLerping = value; }
    }
    private float T;
    private float lerpDuration = 1.0f;
    public Vector3 initialPos; 
    public Quaternion initialRot;

    private CameraMove cameraMove;
    private PlayerMovement_Jerzy playerMovement;

    private PlayerInput playerInput;

    private Vector3 pointA, pointB;
    
    [System.Serializable]
    public class moveNodes
    {
        public Transform nodeA;
        public Transform childA;
        public Transform childB;
    }
    [SerializeField] public moveNodes[] network;

    public LayerMask layer;
    void Start()
    {
        isControllable = false;
         
        newDir = Vector2.zero;

        cameraMove = FindObjectOfType<CameraMove>();

        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();

        playerInput = FindObjectOfType<PlayerInput>();

        pointA = pointB = transform.position; 
        newDir = Vector3.zero;
    }

    void Update()
    {
        if(isLerping)
        {
            cameraMove.canMove = false;
            T += Time.deltaTime / lerpDuration;

            //lerping
            Camera.main.transform.position = Vector3.Lerp(initialPos, cameraPoint.position, T);
            Camera.main.transform.rotation = Quaternion.Slerp(initialRot, cameraPoint.rotation, T);

            if (T >= lerpDuration)
            {
                playerMovement.LockPlayerMovement();

                isLerping = false;
                IsControllable = true;
                playerMovement.usingWand = true;
                T = 0;
            }
        }
        else if(isControllable)
        {
            newDir =  (Quaternion.Euler(transform.rotation.eulerAngles) * playerInput.GetMovementVector()).normalized;

            if(newDir != Vector3.zero)
            foreach (var item in network)
            {
                if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(item.nodeA.position.x, item.nodeA.position.z)) < 0.06f)
                {
                    pointA = item.nodeA.position;
                    float dot1 = 0;
                    float dot2 = 0;
                    if (item.childA != null) dot1 = Vector2.Dot(new Vector2(newDir.x, newDir.z), new Vector2(item.childA.position.x - item.nodeA.position.x, item.childA.position.z - item.nodeA.position.z));
                    if (item.childB != null) dot2 = Vector2.Dot(new Vector2(newDir.x, newDir.z), new Vector2(item.childB.position.x - item.nodeA.position.x, item.childB.position.z - item.nodeA.position.z));
                    if (item.childA != null && dot1 > dot2)
                        pointB = item.childA.position;
                    else if (item.childB != null && dot1 < dot2)
                        pointB = item.childB.position;

                    break;
                }
            }
            
            Vector3 moveToB = (pointB - pointA).normalized;
            Vector3 moveToA = (pointA - pointB).normalized;

            if (newDir != Vector3.zero && Vector3.Dot(moveToB, newDir) > 0.1f && Vector2.Dot(new Vector2(moveToB.x, moveToB.z), new Vector2(transform.position.x - pointB.x, transform.position.z - pointB.z)) < 0 )
            { 
                float Distance = (moveToB.x != 0.0f ? GetComponent<BoxCollider>().size.x / 2.0f : moveToB.z != 0.0f ? GetComponent<BoxCollider>().size.z / 2.0f : 0.0f) + 0.9f; 
                Vector3 leftRight = Vector3.Cross(moveToB, Vector3.up) * 1.15f;
                GetComponent<BoxCollider>().enabled = false;
                RaycastHit hit1; Physics.Raycast(transform.position - leftRight, moveToB, out hit1, Distance);
                RaycastHit hit2; Physics.Raycast(transform.position + leftRight, moveToB, out hit2, Distance);
                GetComponent<BoxCollider>().enabled = true;
                RaycastHit[] hits = { hit1, hit2 }; 
                bool canMove = true;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform && hits[i].transform.tag.Contains("MagneticObj"))
                    {
                        canMove = false;
                        break;
                    }    
                }
                if (canMove) transform.Translate(moveToB * movementSpeed * Time.deltaTime, Space.World);
            }
            else if(newDir != Vector3.zero && Vector3.Dot(moveToB, newDir) < -0.1f && Vector2.Dot(new Vector2(moveToA.x, moveToA.z), new Vector2(transform.position.x - pointA.x, transform.position.z - pointA.z)) < 0)
            { 
                float Distance = (moveToA.x != 0.0f ? GetComponent<BoxCollider>().size.x / 2.0f : moveToA.z != 0.0f ? GetComponent<BoxCollider>().size.z / 2.0f : 0.0f) + 0.9f; 
                Vector3 leftRight = Vector3.Cross(moveToA, Vector3.up) * 1.15f;
                GetComponent<BoxCollider>().enabled = false;
                RaycastHit hit1; Physics.Raycast(transform.position - leftRight, moveToA, out hit1, Distance);
                RaycastHit hit2; Physics.Raycast(transform.position + leftRight, moveToA, out hit2, Distance);
                GetComponent<BoxCollider>().enabled = true;
                RaycastHit[] hits = { hit1, hit2 }; 
                bool canMove = true;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform && hits[i].transform.tag.Contains("MagneticObj"))
                    {
                        canMove = false;
                        break;
                    }
                }
                if (canMove) transform.Translate(moveToA * movementSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isControllable)
        {
            Camera.main.transform.position = cameraPoint.position;
            Camera.main.transform.rotation = cameraPoint.rotation;
        }
    }


    public void StopInteraction()
    {
        if (isControllable)
        {
            //Reset cursor visibility
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //Reset camera pos
            Camera.main.transform.position = initialPos;
            Camera.main.transform.rotation = initialRot;

            //Exit controllable state
            isControllable = false;

            cameraMove.canMove = true;

            playerMovement.usingWand = false;
        }
    }
}
