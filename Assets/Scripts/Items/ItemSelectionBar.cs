using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionBar : MonoBehaviour
{
    private Animator anim;

    private float countdownMax = 5.0f;
    public float countdown = 0f;
    public bool hidden = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        else
        {
            anim.SetBool("Hidden", true);
            hidden = true;
        }
    }
    public void HoverEnter()
    {
        if (!hidden)
            countdown = countdownMax;
    }

    public void ShowHotbar()
    {
        if (UIManager.instance.GetCurrentInput() == UIManager.INPUT_OPTIONS.KEYBOAD_AND_MOUSE)
        {
            countdown = countdownMax;
            anim.SetBool("Hidden", false);
            hidden = false;
        }
    }
}
