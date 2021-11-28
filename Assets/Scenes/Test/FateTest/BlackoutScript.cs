using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutScript : MonoBehaviour
{
    private LoadScene loadScene;
    private Animator blackoutAnimator;

    private int sceneIndex;

    private void Start()
    {
        loadScene = GetComponent<LoadScene>();
        blackoutAnimator = GetComponent<Animator>();
    }
    public void FadeOutOfScene(int index)
    {
        sceneIndex = index;
        blackoutAnimator.SetTrigger("FadeBlack"); //Event within animation triggers GetLoadingScreen();
    }   

    public void GetLoadingScreen()
    {
        loadScene.Load(sceneIndex);
    }
}