using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public int sceneIndex;

    private BlackoutScript blackoutScreen;

    private void Awake()
    {
        blackoutScreen = FindObjectOfType<BlackoutScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats stats))
        {
            stats.SaveStats();

            // Dominique, Use fade to black screen
            blackoutScreen.FadeOutOfScene(sceneIndex);
        }
    }
}
