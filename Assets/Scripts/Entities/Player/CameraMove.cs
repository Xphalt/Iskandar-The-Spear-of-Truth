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

    public bool centred = true; 
    private bool XFollowing, ZFollowing;

    private Camera thisCamera;

    public List<Transform> midGrounds;
    public List<float> midGroundRatios;

    private Vector3 Limits;   
    public List<Vector3> midGroundStarts;



    private void Awake()
    {
        thisCamera = GetComponent<Camera>();
        Limits = thisCamera.ScreenToWorldPoint(new Vector3(Screen.width, thisCamera.transform.position.y, Screen.height));
        if (centred)    Limits -= transform.position;
        foreach (Transform mg in midGrounds)    midGroundStarts.Add(mg.position);
    }

    void Update()
    {
        XFollowing = RightBound.position.x - LeftBound.position.x > Limits.x * 2;
        ZFollowing = UpBound.position.z - DownBound.position.z > Limits.z * 2;

        float Xmid = (RightBound.position.x + LeftBound.position.x) / 2;
        float Zmid = (UpBound.position.z + DownBound.position.z) / 2;

        float newX = XFollowing ? Mathf.Clamp(Target.position.x, LeftBound.position.x + Limits.x, RightBound.position.x - Limits.x) : Xmid;
        float newZ = ZFollowing ? Mathf.Clamp(Target.position.z, DownBound.position.z + Limits.z, UpBound.position.z - Limits.z) : Zmid;

        transform.position = new Vector3(newX, transform.position.y, newZ);

        for (int m = 0; m < midGrounds.Count; m++)
        {
            Vector3 newMidGround = Vector3.Lerp(midGroundStarts[m], transform.position, midGroundRatios[m]);
            midGrounds[m].position = new Vector3(newMidGround.x, midGrounds[m].position.y, newMidGround.z);
        }
    }

    
}
