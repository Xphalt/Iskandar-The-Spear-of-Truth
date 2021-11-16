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

    private Vector2 newDir;

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
    private PlayerMovement_Jerzy player;

    void Start()
    {
        isControllable = false;
         
        newDir = Vector2.zero;

        cameraMove = FindObjectOfType<CameraMove>();

        player = FindObjectOfType<PlayerMovement_Jerzy>();
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
                isLerping = false;
                IsControllable = true;
                player.usingWand = true;
                T = 0;
            }
        }
        else if(isControllable)
        {
            newDir = Mouse.current.delta.ReadValue().normalized; 

            transform.position += new Vector3(newDir.x, 0.0f, newDir.y) * movementSpeed * Time.deltaTime; 

            if(Mouse.current.leftButton.isPressed)
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

                player.usingWand = false;
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
}
