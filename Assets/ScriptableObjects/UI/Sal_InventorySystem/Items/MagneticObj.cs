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
        if (isLerping)
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
        else if (isControllable)
        {
            newDir = (Quaternion.Euler(transform.rotation.eulerAngles) * playerInput.GetMovementVector()).normalized;
            foreach (var item in network)
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(item.nodeA.position.x, item.nodeA.position.z)) < 0.05f)
                {
                    float dot1 = 0;
                    float dot2 = 0;
                    if (item.childA != null) dot1 = Vector2.Dot(new Vector2(newDir.x, newDir.z), new Vector2(item.childA.position.x, item.childA.position.z));
                    if (item.childB != null) dot2 = Vector2.Dot(new Vector2(newDir.x, newDir.z), new Vector2(item.childB.position.x, item.childB.position.z));
                    pointA = item.nodeA.position;
                    if (item.childA != null && dot1 > dot2)
                        pointB = item.childA.position;
                    else if (item.childB != null && dot1 < dot2)
                        pointB = item.childB.position;
                }
            }

            Vector3 moveToB = (pointB - pointA).normalized;
            Vector3 moveToA = (pointA - pointB).normalized;

            Debug.Log(newDir);

            if (newDir != Vector3.zero && Vector3.Dot(moveToB, newDir) > 0.1f && Vector2.Dot(new Vector2(moveToB.x, moveToB.z), new Vector2(transform.position.x - pointB.x, transform.position.z - pointB.z)) < 0)
            {
                Vector3 mod = new Vector3((GetComponent<BoxCollider>().size.x / 2) * moveToB.x, 0.0f, (GetComponent<BoxCollider>().size.z / 2) * moveToB.z);
                var ray = new Ray(transform.TransformPoint(GetComponent<BoxCollider>().center) + mod, moveToB);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                if (!Physics.Raycast(ray, out RaycastHit hit, 0.05f, layer))
                {
                    Debug.Log("Did not Hit");
                    transform.Translate(moveToB * movementSpeed * Time.deltaTime, Space.World);
                }
            }
            else if (newDir != Vector3.zero && Vector3.Dot(moveToB, newDir) < -0.1f && Vector2.Dot(new Vector2(moveToA.x, moveToA.z), new Vector2(transform.position.x - pointA.x, transform.position.z - pointA.z)) < 0)
            {
                Vector3 mod = new Vector3((GetComponent<BoxCollider>().size.x / 2) * moveToA.x, 0.0f, (GetComponent<BoxCollider>().size.z / 2) * moveToA.z);
                var ray = new Ray(transform.TransformPoint(GetComponent<BoxCollider>().center) + mod, moveToA);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

                if (!Physics.Raycast(ray, out RaycastHit hit, 0.05f, layer))
                {
                    Debug.Log("Did not Hit");
                    transform.Translate(moveToA * movementSpeed * Time.deltaTime, Space.World);
                }
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
