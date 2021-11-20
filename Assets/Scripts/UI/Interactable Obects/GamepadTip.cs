using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*__________________________________________________________________________________
 * This script reveals a UI hint on the interaction controls and changes it 
 * according to its type. This was created by Fate, contact me if you need any help.
 *__________________________________________________________________________________*/

public class GamepadTip : MonoBehaviour
{
    private Image gamepadUiImage;
    private CanvasGroup canvasGroup;

    public float fadeDuration = 0.4f;
    public GameObject gamepadUi;
    public Sprite readSprite, talkSprite;

    void Awake()
    {
        gamepadUiImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void DisplayGamepadUI(string type)
    {
        //Change sprite

        if (gamepadUi.activeInHierarchy && type == "read")
            gamepadUiImage.sprite = readSprite;
        else if (gamepadUi.activeInHierarchy && type == "talk")
            gamepadUiImage.sprite = talkSprite;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(canvasGroup.alpha, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(canvasGroup.alpha, 0));
    }

    public IEnumerator Fade(float startAlphaValue, float endAlphaValue)
    {
        float timeStart = Time.time;
        float timeSinceStart;
        float percentageComplete, currentValue;

        while (true)
        {
            timeSinceStart = Time.time - timeStart;
            percentageComplete = timeSinceStart / fadeDuration;

            currentValue = Mathf.Lerp(startAlphaValue, endAlphaValue, percentageComplete);
            canvasGroup.alpha = currentValue;

            //When alpha = 1, break loop
            if (percentageComplete >= 1) break;

            print("current value" + currentValue);
                 
            yield return new WaitForEndOfFrame();
        }
    }
}