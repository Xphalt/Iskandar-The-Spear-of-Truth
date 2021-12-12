using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    // Public vars
    [Tooltip("Seconds to wait before scrolling begins")]
    public float initialWaitTime = 2;
    [Tooltip("Seconds to wait after scrolling ends")]
    public float endWaitTime = 3;
    [Tooltip("Duration of credits, including start and end wait time")]
    public float creditsDuration = 80;
    [Tooltip("Animation curve of credits scroll speed")]
    public AnimationCurve accelerationCurve;

    public Transform screenTop, screenBottom, namesParent, creditsBottom;

    private void Start()
    {
        // Set initial positions
        transform.localPosition = Vector3.zero;
        namesParent.position = screenBottom.position;

        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(initialWaitTime);

        // smoothly scroll from beginning to end of credits in the given time
        Vector3 startPos = transform.position;
        float scrollDistance = Vector3.Distance(creditsBottom.position, startPos);
        float scrollDuration = creditsDuration - (initialWaitTime + endWaitTime);
        for (float t = 0; t < scrollDuration; t += Time.deltaTime)
        {
            yield return null;

            float value = accelerationCurve.Evaluate(t / scrollDuration);
            float newYPos = Mathf.Lerp(startPos.y, startPos.y + scrollDistance, value);

            //float newYPos = Mathf.SmoothStep(startPos.y, startPos.y + scrollDistance, t / scrollDuration);
            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
        }

        yield return new WaitForSeconds(endWaitTime);
        SceneManager.LoadScene(0);
    }
}
