using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionWheel : MonoBehaviour
{
    public Animator anim;
   
    [HideInInspector] public int itemID = -1;
    [HideInInspector] public bool itemWheelSelected = false;
 
    public bool IsItemWheelSelected() { return itemWheelSelected; }
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
    public void UseItem()
    {
        // Use selected item or potion (they are on seperate wheels)
    }        
}
