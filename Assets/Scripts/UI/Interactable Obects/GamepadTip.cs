using UnityEngine;
using UnityEngine.UI;

public class GamepadTip : MonoBehaviour
{
    public GameObject gamepadUi;
    private Image gamepadUiImage;

    public Sprite readSprite, talkSprite;


    void Start()
    {
        //gamepadUi = GetComponentInParent<GameObject>();
        gamepadUiImage = GetComponent<Image>();
      ///  gamepadUi = GameObject.Find("GamepadOnlyUI");
    }

    public void DisplayGamepadUI(string type)
    {
        //make sprite visible
        if (gamepadUi.activeInHierarchy && type == "read")
        {
            gamepadUiImage.sprite = readSprite;
        }
        else if (gamepadUi.activeInHierarchy && type == "talk")
        {
            gamepadUiImage.sprite = talkSprite;
        }
    }
}