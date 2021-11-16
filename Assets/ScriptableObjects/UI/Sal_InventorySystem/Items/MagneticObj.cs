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

    void Start()
    {
        isControllable = false;
         
        newDir = Vector2.zero;

        cameraMove = FindObjectOfType<CameraMove>();

        playerMovement = FindObjectOfType<PlayerMovement_Jerzy>();

        playerInput = FindObjectOfType<PlayerInput>();
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
            newDir =  playerInput.GetMovementVector().normalized; 

            transform.position += newDir * movementSpeed * Time.deltaTime; 
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
