using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutScript : MonoBehaviour
{
    private LoadScene loadScene;
    private Animator blackoutAnimator;
    private void Start()
    {
        loadScene = GetComponent<LoadScene>();
        blackoutAnimator = GetComponent<Animator>();
    }
    //public void FadeOutOfScene()//int sceneIndex)
    //{
        
    //    StartCoroutine(DelayLoadingScreen());// sceneIndex));
    //}
    public IEnumerator GetLoadingScreen()//int sceneIndex)
    {
        blackoutAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(2); //get length of fade animation
        loadScene.Load();// sceneIndex);
    }
}
