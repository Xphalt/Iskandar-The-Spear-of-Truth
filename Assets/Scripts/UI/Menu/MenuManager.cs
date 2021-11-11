using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public GameObject[] saveSelections;
    public Sprite emptyIcon;

    public GameObject inputName;
    public GameObject continueDeletePanel;

    int currentSaveFileSelected = 0;

    // Populate the locale dropdown
    IEnumerator Start()
    {
        UpdateSavePanel();

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
        //Load Saves

        //For loop save file

        /*
         * {
         *      saveSelection[i].transform.getChild(0).getChild(0).getComponent<TextMeshProGUI>().text = saveFile.name?
         *      saveSelection[i].transform.getChild(1).getComponent<Image>().sprite = saveFile.icon?    
         *      saveSelections[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ShowContinueDelete);
         * }
         * 
         * For loop saveSelection.length - saveFile.length
         * {
         *      saveSelection[i].transform.getChild(0).getChild(0).getComponent<TextMeshProGUI>().text = "Empty"
         *      saveSelection[i].transform.getChild(1).getComponent<Image>().sprite = emptyIcon
         *      saveSelections[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ShowInputField);
         * }
         * 
         */
    }

    public void ShowContinueDelete()
    {
        continueDeletePanel.SetActive(true);
    }

    public void SetCurrentSaveFile(int file)
    {
        currentSaveFileSelected = file;
    }

    public void ShowInputField()
    {
        inputName.SetActive(true);
    }

    public void StartGame()
    {
        //Start
    }

    public void GetSaveFiles(int num)
    {
        //SaveManager.LoadPlayerStats(num).
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
