using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    // Public vars
    [Tooltip("Seconds to wait before scrolling begins")]
    public float InitialWaitTime = 2;
    [Tooltip("Speed to scroll credits in units/second")]
    public float scrollSpeed = 1;

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
        yield return new WaitForSeconds(InitialWaitTime);

        // Move credits text up at scrollSpeed
        float currentScrollSpeed = 0;
        float acceleration = 0;
        float secondsToAccelerate = 1;
        while (creditsBottom.position.y <= screenTop.position.y)
        {
            // Initial acceleration
            yield return null;
            if(acceleration < 1)
            {
                acceleration += Time.deltaTime * secondsToAccelerate;
                currentScrollSpeed = Mathf.SmoothStep(0, scrollSpeed, acceleration);
            }

            transform.Translate(Vector3.up * currentScrollSpeed * Time.deltaTime);
        }

        // When credits have scrolled off screen, go to main menu
        SceneManager.LoadScene(0);
    }
}
