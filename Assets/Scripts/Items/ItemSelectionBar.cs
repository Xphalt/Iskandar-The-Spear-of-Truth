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
        if (Input.GetKeyDown(KeyCode.Alpha1) || (Input.GetKeyDown(KeyCode.Alpha2) || (Input.GetKeyDown(KeyCode.Alpha3))))
        {
            countdown = countdownMax;
            anim.SetBool("Hidden", false);
            hidden = false;
        }

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
}
