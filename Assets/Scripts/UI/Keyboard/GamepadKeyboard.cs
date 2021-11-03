using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamepadKeyboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject upperCaseButtons;
    [SerializeField] private GameObject lowerCaseButtons;
    [SerializeField] private Button startKey;

    bool capitalised = false;

    private void Start()
    {
        if (capitalised)
        {
            ToggleShift();
        }
        startKey.Select();
    }

    public void AddCharacter(GameObject button)
    {
        if (inputField.text.Length < inputField.characterLimit)
        {
            inputField.text += button.GetComponentInChildren<TextMeshProUGUI>().text;
            if (capitalised)
            {
                ToggleShift();
                startKey.Select();
            }
            else
            {
                button.GetComponent<Button>().Select();
            }
        }
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void Space()
    {
        if (inputField.text.Length < inputField.characterLimit)
        {
            inputField.text += ' ';
        }
    }

    public void ToggleShift()
    {
        capitalised = !capitalised;
        if (capitalised)
        {
            lowerCaseButtons.SetActive(false);
            upperCaseButtons.SetActive(true);
        }
        else
        {
            lowerCaseButtons.SetActive(true);
            upperCaseButtons.SetActive(false);
        }
    }
}
