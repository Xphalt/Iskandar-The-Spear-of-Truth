using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public string qualitySettingsStringTable = "QualitySettings";

    public Dictionary<string, SaveData> saveSelections = new Dictionary<string, SaveData>();
    public Transform[] buttons;
    public Sprite emptyIcon;

    public GameObject inputName;
    public GameObject continueDeletePanel;
    public GameObject startPanel;

    internal SaveData currentSaveFile;

    public PlayerNameManager pnm;

    [SerializeField] private LoadScene loadScene;

    private bool validName;

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

        GameObject.Find("Play").GetComponent<Button>().Select();
    }


    // Update the locale when changed with dropdown
    public void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public List<int> listOfNumsNotUsed = new List<int>();

    public void UpdateSavePanel()
    {
        startPanel.SetActive(true);
        saveSelections.Clear();
        GetSaveFiles();

        listOfNumsNotUsed.Clear();

        for (int i = 0; i < buttons.Length; i++)
        {
            listOfNumsNotUsed.Add(i);
        }

        for (int i = 0; i < saveSelections.Count; i++)
        {
            //Debug.Log(saveSelections.ElementAt(i).Key);

            //Gets player name and sets it to text
            buttons[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = saveSelections.ElementAt(i).Key;

            //Sets the quest icon?!?!??!?!?!?
            //saveSelection[i].transform.getChild(1).getComponent<Image>().sprite = saveFile.icon ?

            //When button's clicked, show continue and delete buttons
            buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(ShowContinueDelete);

            //When button's clicked, hides the input field for new name
            buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(HideInputField);

            //When button's clicked, sets the current save num variable to the prewritten save num in the save data
            buttons[i].GetChild(0).GetComponent<MenuSaveIdentity>().id = saveSelections.ElementAt(i).Value.LastFileSaved;

            //Removes the numbers that are being used by save files
            listOfNumsNotUsed.Remove(saveSelections.ElementAt(i).Value.LastFileSaved);
        }


        for (int i = saveSelections.Count; i < buttons.Length; i++)
        {
            //Set's the button text to empty as no save file exists at this ID
            buttons[i].GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Empty";

            //Set's 'quest' icon to empty icon
            //saveSelection[i].transform.getChild(1).getComponent<Image>().sprite = emptyIcon

            //Shows the input name field
            buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(ShowInputField);

            //Hides the ability to continue a save or delete a save, as it doesn't exist at the current ID
            buttons[i].GetChild(0).GetComponent<Button>().onClick.AddListener(HideContinueDelete);

            buttons[i].GetChild(0).GetComponent<MenuSaveIdentity>().id = listOfNumsNotUsed[i - saveSelections.Count];
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
        if (validName)
        {
            FindObjectOfType<SaveDataAssistant>().currentSaveFileID = pnm.currentSaveFile;
            loadScene.gameObject.SetActive(true);

            try
            {
                currentSaveFile = saveSelections.ElementAt(pnm.currentSaveFile).Value;
                loadScene.Load(currentSaveFile.sceneEventIndex);
            }
            catch (System.Exception)
            {
                loadScene.Load(1);
            }
        }
        else Debug.LogWarning("Invalid Name");
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
                fs.Close();
                fs = new FileStream(namePath, FileMode.Open);

                name = bf.Deserialize(fs) as string;

                saveSelections.Add(name, save);
                fs.Close();
            }       
        }               
    }

    public void DeleteSaveFile()
    {
        int i = pnm.currentSaveFile;

        string saveDataPath = Application.persistentDataPath + "/Player_statsf" + i + ".txt";
        string namePath = Application.persistentDataPath + "/Player_name" + i + ".txt";
        string invPath = Application.persistentDataPath + "/Player_Inventoryf" + i + ".txt";

        if (File.Exists(saveDataPath))
        {
            File.Delete(saveDataPath);
        }
        if (File.Exists(namePath))
        {
            File.Delete(namePath);
        }
        if (File.Exists(invPath))
        {
            File.Delete(invPath);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ValidNameCheck(string name)
    {
        validName = true;
        for (int i = 0; i < pnm.invalidCharacters.Length; i++)
        {
            if (name.Contains(pnm.invalidCharacters[i]))
            {
                validName = false;
            }
        }

        if(name.Length <= 0)
        {
            validName = false;
        }
    }
}
