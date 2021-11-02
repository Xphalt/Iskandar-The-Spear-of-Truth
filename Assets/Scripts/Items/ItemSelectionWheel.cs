using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionWheel : MonoBehaviour
{
    public EquipPanel equipment;
    public Animator anim;
    private bool itemWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int itemID;
    public ItemWheelButtonController[] itemWheelButtons;

    private void Update()
    {

        if (itemWheelSelected)
        {
            anim.SetBool("OpenItemWheel", true);
        }
        else
        {
            anim.SetBool("OpenItemWheel", false);
        }
    }

    public void ToggleItemSelectionWheel()
    {
        itemWheelSelected = !itemWheelSelected;
    }

    public void ShowItem(Item item)
    {
        //itemWheelButtons[item.id].icon = item.icon?????
    }        
}
