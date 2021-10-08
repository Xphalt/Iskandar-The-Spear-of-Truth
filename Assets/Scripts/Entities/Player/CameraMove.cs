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
    public float Yoffset = 10, Zoffset = 5;

    public Vector3 Limits = new Vector3(15, 0, 5);   


    private void Awake() //used to avoid reference errors
    {
        transform.position = new Vector3(Target.position.x, Yoffset, Zoffset);
    }

    void Update()
    {
        XFollowing = RightBound.position.x - LeftBound.position.x > Limits.x * 2; //Can be done in awake if boundaries do not change mid-level
        ZFollowing = UpBound.position.z - DownBound.position.z > Limits.z * 2;

        float Xmid = (RightBound.position.x + LeftBound.position.x) / 2;
        float Zmid = (UpBound.position.z + DownBound.position.z) / 2;

        float newX = XFollowing ? Mathf.Clamp(Target.position.x, LeftBound.position.x + Limits.x, RightBound.position.x - Limits.x) : Xmid;
        float newZ = ZFollowing ? Mathf.Clamp(Target.position.z, DownBound.position.z + Limits.z, UpBound.position.z - Limits.z) : Zmid;

        transform.position = new Vector3(newX, Yoffset, (newZ - Zoffset));
    }    
}
