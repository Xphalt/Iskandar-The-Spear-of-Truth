using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionWheel : MonoBehaviour
{
    public Animator anim;
    private bool itemWheelSelected = false;
    public bool IsItemWheelSelected() { return itemWheelSelected; }
    [HideInInspector] public int itemID = -1;

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

    public void UseItem()
    {
        // Use selected item or potion (they are on seperate wheels)
    }        
}
