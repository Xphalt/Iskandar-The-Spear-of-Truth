using UnityEngine;
using UnityEngine.UI;

//This script was created by Fate, contact me if you need any help.

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;
    private Image toolTipImage;

    private void Awake()
    {
        //If there is more than one instance of ToolTipManager in the scene
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        toolTipImage = GetComponent<Image>();
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowToolTip(Sprite image)
    {
        gameObject.SetActive(true);
        toolTipImage.sprite = image;
        toolTipImage.enabled = true;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        toolTipImage.enabled = false;
    }
}
