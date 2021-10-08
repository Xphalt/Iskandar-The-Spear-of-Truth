using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform UpBound;
    public Transform DownBound;
    public Transform LeftBound;
    public Transform RightBound;
    public Transform Target;

    private bool XFollowing, ZFollowing;
    public float Zoffset;

    private Camera thisCamera;

    public Vector3 Limits;   


    private void Awake() //used to avoid reference errors
    {
        thisCamera = GetComponent<Camera>();       
        //Limits = new Vector3(0, thisCamera.transform.position.y, 0);
    }

    void Update()
    {
        
        XFollowing = RightBound.position.x - LeftBound.position.x > Limits.x * 2;
        ZFollowing = UpBound.position.z - DownBound.position.z > Limits.z * 2;

        float Xmid = (RightBound.position.x + LeftBound.position.x) / 2;
        float Zmid = (UpBound.position.z + DownBound.position.z) / 2;

        float newX = XFollowing ? Mathf.Clamp(Target.position.x, LeftBound.position.x + Limits.x, RightBound.position.x - Limits.x) : Xmid;
        float newZ = ZFollowing ? Mathf.Clamp(Target.position.z, DownBound.position.z + Limits.z, UpBound.position.z - Limits.z) : Zmid;

        transform.position = new Vector3(newX, transform.position.y, (newZ - Zoffset));

    }

    
}
