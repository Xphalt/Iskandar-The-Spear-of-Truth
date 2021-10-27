using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject[] saveSelections;
    public Sprite emptyIcon;

    public GameObject inputName;
    public GameObject continueDeletePanel;

    int currentSaveFileSelected = 0;

    private void Start()
    {
        UpdateSavePanel();
    }

    private void FixedUpdate()
    {
       
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
