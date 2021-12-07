using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutScript : MonoBehaviour
{
    private LoadScene loadScene;
    private Animator blackoutAnimator;

    private int sceneIndex;

    private void Awake()
    {
        loadScene = GetComponent<LoadScene>();
        blackoutAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }
    public void FadeOutOfScene(int index)
    {
        gameObject.SetActive(true);
        sceneIndex = index;
        blackoutAnimator.SetTrigger("FadeBlack"); 
    }

    public void GetLoadingScreen()
    {
        //Event within animation triggers GetLoadingScreen();
        loadScene.Load(sceneIndex);
    }
}