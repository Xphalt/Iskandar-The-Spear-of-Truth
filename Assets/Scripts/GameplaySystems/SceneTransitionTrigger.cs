using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStats stats))
        {
            stats.SaveStats();

            // Dominique, Use fade to black screen
            UIManager.instance.GetBlackoutScreen().gameObject.SetActive(true);
            UIManager.instance.GetBlackoutScreen().FadeOutOfScene(sceneIndex);
        }
    }
}
