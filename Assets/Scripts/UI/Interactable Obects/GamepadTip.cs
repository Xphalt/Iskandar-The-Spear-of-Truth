using UnityEngine;
using UnityEngine.UI;

/*__________________________________________________________________________________
 * This script reveals a UI hint on the interaction controls and changes it 
 * according to its type. This was created by Fate, contact me if you need any help.
 *__________________________________________________________________________________*/

public class GamepadTip : MonoBehaviour
{
    public GameObject gamepadUi;
    private Image gamepadUiImage;

    public Sprite readSprite, talkSprite;

    void Awake()
    {
        gamepadUiImage = GetComponent<Image>();
    }

    public void DisplayGamepadUI(string type)
    {
        //Change sprite

        if (gamepadUi.activeInHierarchy && type == "read")
            gamepadUiImage.sprite = readSprite;
        else if (gamepadUi.activeInHierarchy && type == "talk")
            gamepadUiImage.sprite = talkSprite;
    }
}