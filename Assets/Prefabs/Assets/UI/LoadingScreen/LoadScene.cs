using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressSlider;

    private void Awake()
    {
        //loadingScreen = GameObject.Find("Loading Screen");
       // progressSlider = loadingScreen.GetComponentInChildren<Slider>();
    }

    public void Load(int sceneNumber)
    {
        StartCoroutine(LoadInSync(sceneNumber));
    }

    IEnumerator LoadInSync(int sceneNumber)
    {
        //Save status of loading into operation. This can then be used to get the progress.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
        loadingScreen.SetActive(true);

        if (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressSlider.value = progress;
            yield return null; //Waits until next frame before continuing to loop
        }
    }
}
