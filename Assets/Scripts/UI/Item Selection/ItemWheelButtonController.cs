using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemWheelButtonController : MonoBehaviour
{
    public int id;
    private Animator anim;
    //public string itemName;
    //public TextMeshProUGUI itemText;
    public Image selectedItem;
    //private bool selected = false;
    public Sprite icon;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Selected()
    {
        //selected = true;
        selectedItem.sprite = icon;
        ItemSelectionWheel.itemID = id;
    }

    /*
    public void Deselected()
    {
        //selected = false;
        selectedItem.sprite = null;
        ItemSelectionWheel.itemID = 0;
    }
    */

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        //itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        //itemText.text = "";
    }
}