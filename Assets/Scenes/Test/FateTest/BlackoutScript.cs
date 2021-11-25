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
    public void FadeOutOfScene()//int sceneIndex)
    {
        blackoutAnimator.SetTrigger("FadeBlack"); //Event within animation triggers GetLoadingScreen();
        //StartCoroutine(DelayLoadingScreen());// sceneIndex));
    }

    public void GetLoadingScreen()
    {
        loadScene.Load();// sceneIndex);
    }

//  public IEnumerator GetLoadingScreen()//int sceneIndex)
//  {
//      yield return new WaitForSeconds(2); //get length of fade animation
//       oadScene.Load();// sceneIndex);
//  }
}
