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

    public float panSpeed = 10;
    private float panLinger = 0;
    private float panDuration = 0;
    private float panTimer = 0;

    private bool panning = false;
    private Vector3 panTarget = new Vector3();
    private Vector3 panStart = new Vector3();

    private bool Bound => LeftBound != null && RightBound != null && UpBound != null && DownBound != null;
    private Vector3 TargetPos => new Vector3(Target.position.x, Target.position.y + Yoffset, Target.position.z - Zoffset);

    private void Awake() //used to avoid reference errors
    {
        if (Target == null) Target = FindObjectOfType<PlayerMovement_Jerzy>().transform;
        transform.position = new Vector3(Target.position.x, Yoffset, Zoffset);
    }

    void Update()
    {
        if (!panning) FollowPlayerToLimits();
        else Pan();
    }

    private void FollowPlayerToLimits()
    {
        float newX = Target.position.x, newZ = Target.position.z;

        if (Bound)
        {
            XFollowing = RightBound.position.x - LeftBound.position.x > Limits.x * 2; //Can be done in awake if boundaries do not change mid-level
            ZFollowing = UpBound.position.z - DownBound.position.z > Limits.z * 2;

            float Xmid = (RightBound.position.x + LeftBound.position.x) / 2;
            float Zmid = (UpBound.position.z + DownBound.position.z) / 2;

            newX = XFollowing ? Mathf.Clamp(Target.position.x, LeftBound.position.x + Limits.x, RightBound.position.x - Limits.x) : Xmid;
            newZ = ZFollowing ? Mathf.Clamp(Target.position.z, DownBound.position.z + Limits.z, UpBound.position.z - Limits.z) : Zmid;
        }
        
        transform.position = new Vector3(newX, Target.position.y + Yoffset, newZ - Zoffset);
    }

    public void StartPan(Vector3 newPan, float linger)
    {
        panTarget = new Vector3(newPan.x, newPan.y + Yoffset, newPan.z - Zoffset);
        panStart = transform.position;
        panLinger = linger;
        panDuration = panStart.GetDistance(panTarget) / panSpeed;
        panTimer = 0;
        panning = true;
    }

    public void StartPan(Transform newPan, float linger = -1)
    {
        StartPan(newPan.position, linger);
    }

    public void EndPan()
    {
        panLinger = 0;
        panTarget = transform.position;
        panDuration = panStart.GetDistance(panTarget) / panSpeed;
        panTimer = panDuration;
    }

    private void Pan()
    {
        panTimer += Time.deltaTime;

        if (panTimer < panDuration)
            transform.position = Vector3.Lerp(panStart, panTarget, Mathf.SmoothStep(0, 1, panTimer / panDuration));
        else if (panLinger >= 0)
        {
            if (panTimer > panDuration + panLinger && panTimer < panDuration * 2 + panLinger)
                transform.position = Vector3.Lerp(panTarget, TargetPos, Mathf.SmoothStep(0, 1, (panTimer - panDuration - panLinger) / panDuration));
            else if (panTimer > panDuration * 2 + panLinger)
                panning = false;
        }
    }

    public void TestPan()
    {
        StartPan(UpBound);
    }
}
