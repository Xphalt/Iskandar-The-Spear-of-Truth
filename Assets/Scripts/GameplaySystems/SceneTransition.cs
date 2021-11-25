using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private LoadScene LoadSceneManager;

    [SerializeField] LayerMask playerLayer;
    enum Scenes
    {
        Village,
        Forest,
        ForestDungeon,
        Desert,
        DesertDungeon,
        FinalDungeon
    }

    [SerializeField] Scenes nextArea;

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.layer);
        if (collision.gameObject.layer == playerLayer)
        {
            LoadSceneManager.Load();  //TELL LEWIS THAT THE SCENE NAME IS UNNEEDED 
        }
    }
}
