using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraFeature : MonoBehaviour
{
    /* This script should be added to an object with a trigger and a kinematic rigidbody. 
     * When player enters this trigger, camera will transition to alternate angle.
     * No two triggers with this script should intersect.
     * Assign the cam variable of this script in editor.
     */

    // Public vars
    public float altYOffset = 13.46f, altZOffset = 5.33f, transitionDuration = 0.5f;
    public CameraMove cam;
    // Private vars
    float defaultYOffset, defaultZOffset;

    private void Start()
    {
        defaultYOffset = cam.Yoffset;
        defaultZOffset = cam.Zoffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When player enters trigger, transition to alternate camera angle
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(TransitionCamAngle(altYOffset, altZOffset));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When player leaves trigger, transition to default camera angle
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(TransitionCamAngle(defaultYOffset, defaultZOffset));
        }
    }

    IEnumerator TransitionCamAngle(float targetY, float targetZ)
    {
        // Move camera to target position in specified amount of time, with eased movement
        float startingY = cam.Yoffset, startingZ = cam.Zoffset;
        for (float t = 0; t < 1; t+= Time.deltaTime / transitionDuration)
        {
            yield return null;
            float smoothValue = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(t);
            cam.Yoffset = Mathf.Lerp(startingY, targetY, smoothValue);
            cam.Zoffset = Mathf.Lerp(startingZ, targetZ, smoothValue);
        }
        cam.Yoffset = targetY;
        cam.Zoffset = targetZ;
    }
}
