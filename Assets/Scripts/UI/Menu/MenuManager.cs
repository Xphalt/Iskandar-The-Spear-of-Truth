using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public Dictionary<string, SaveData> saveSelections = new Dictionary<string, SaveData>();
    public Transform[] buttons;
    public Sprite emptyIcon;

    public GameObject inputName;
    public GameObject continueDeletePanel;
    public GameObject startPanel;

    // Populate the locale dropdown
    IEnumerator Start()
    {
        GetSaveFiles();

        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.NativeName));
        }
        dropdown.options = options;

        dropdown.value = selected;
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }
    // Update the locale when changed with dropdown
    public void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public void UpdateSavePanel()
    {
        startPanel.SetActive(true);

        for (int i = 0; i < saveSelections.Count; i++)
        {
           buttons[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = saveSelections.ElementAt(i).Key;
           //saveSelection[i].transform.getChild(1).getComponent<Image>().sprite = saveFile.icon ?
           buttons[i].GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
           buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(ShowContinueDelete);
           buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(HideInputField);
         }

        Debug.Log(buttons.Length + " - " + saveSelections.Count + " = " + (buttons.Length - saveSelections.Count));

        for (int i = saveSelections.Count; i < buttons.Length - saveSelections.Count; i++)
        {
            buttons[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Empty";
            //saveSelection[i].transform.getChild(1).getComponent<Image>().sprite = emptyIcon
            buttons[i].GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(ShowInputField);
            buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(HideContinueDelete);
        }       
    }

    public void ShowContinueDelete()
    {
        continueDeletePanel.SetActive(true);
    }
    public void HideContinueDelete()
    {
        continueDeletePanel.SetActive(false);
    }

    public void ShowInputField()
    {
        inputName.SetActive(true);
    }

    public void HideInputField()
    {
        inputName.SetActive(false);
    }

    public void StartGame()
    {
    /*if ()
        {

        }
        else
        {*/
            SceneManager.LoadScene("Production"); // place holder
        //}
    }

    public void GetSaveFiles()
    {
        for (int i = 0; i < 5; i++)
        {
            SaveData save;
            string name;

            string saveDataPath = Application.persistentDataPath + "/Player_statsf" + i + ".txt";
            string namePath = Application.persistentDataPath + "/Player_name" + i + ".txt";
            if (File.Exists(saveDataPath) && File.Exists(namePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(saveDataPath, FileMode.Open);

                save = bf.Deserialize(fs) as SaveData;

                fs = new FileStream(namePath, FileMode.Open);

                name = bf.Deserialize(fs) as string;

                saveSelections.Add(name, save);
                fs.Close();
                Debug.Log(name + " " + save);
            }       
        }               
    }

    public void DeleteSaveFile()
    {
        //currentSaveFileSelected
    }

    public void QuitGame()
    {
        Application.Quit();
    }  
}
