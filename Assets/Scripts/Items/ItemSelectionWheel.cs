using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionWheel : MonoBehaviour
{
#if UNITY_STANDALONE

#endif


#if UNITY_ANDROID

#endif


    public Animator anim;
    private bool itemWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int itemID;

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
        
}
