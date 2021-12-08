using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosManager : MonoBehaviour
{
    public List<string> previousSceneNames = new List<string>();
    public List<Transform> startPositions = new List<Transform>();

    public void SetPos(string sceneName)
    {
        if (!previousSceneNames.Contains(sceneName)) return;

        transform.position = startPositions[previousSceneNames.IndexOf(sceneName)].position;
    }
}
