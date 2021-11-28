using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script was made by Fate, contact me if you need any help with it.

//ASK LEWIS WHAT CHANGES HE MADE BEFORE COMMITTING, DON'T FORGET!

public class LoadScene : MonoBehaviour
{
    private GameObject loadingScreen;
    private Slider progressSlider;
    private Text progressText;
    private void Awake()
    {
        loadingScreen = gameObject.transform.GetChild(0).gameObject; //Don't change prefab order
        progressSlider = loadingScreen.GetComponentInChildren<Slider>();
        progressText = loadingScreen.GetComponentInChildren<Text>();
    }
    public void Load(int sceneNumber)
    {
        StartCoroutine(LoadInSync(sceneNumber));
    }

    IEnumerator LoadInSync(int sceneNumber)
    {
        /*_____________________________________________________________________________________________________
         * This line loads the next scene rather than inputting the scene number. I've commented this out for
         * testing purposes.
         * _____________________________________________________________________________________________________*/
        //AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //______________________________________________________________________________________________________

        //Save status of loading into operation. This can then be used to get the progress.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float progressPercent = progress * 100;

            progressText.text = progressPercent.ToString("F0") + " %"; //F0 removes decimals
            progressSlider.value = progress;
            yield return null; //Waits until next frame before continuing to loop
        }
    }
}
