using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemWheelButtonController : MonoBehaviour
{
    public int id;
    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        ItemSelectionWheel.itemID = id;
    }

    public void Deselected()
    {
        selected = false;
        ItemSelectionWheel.itemID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }

}
