using UnityEngine;

public class Screenshot : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("insert camera shutter sfx here!");
            ScreenCapture.CaptureScreenshot("Assets/Screenshot.png", 1);
        }
    }
}
