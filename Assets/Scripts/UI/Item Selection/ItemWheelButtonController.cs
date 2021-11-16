using UnityEngine;
using UnityEngine.UI;

public class ItemWheelButtonController : MonoBehaviour
{
    public enum ITEM_TYPE
    { 
        ITEM,
        POTION,
    }

    public ITEM_TYPE item_type;

    public int id;
    private Animator anim;
    public Image selectedItem;
    public Sprite icon;
    [SerializeField] private ItemSelectionWheel itemSelectionWheel;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Selected()
    {
        itemSelectionWheel.itemID = id;
        // Potions don't update the selected item image since they are just used and not selected
        switch (item_type)
        {
            case ITEM_TYPE.ITEM:
                selectedItem.sprite = icon;
                itemSelectionWheel.itemID = id;
                break;
            case ITEM_TYPE.POTION:
                itemSelectionWheel.UseItem();
                break;
        }
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
    }
}