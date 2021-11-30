using UnityEngine;

/*
 * Dominique 11-11-2021,
 * Add functionality to open/close the ItemSelectionwheel if held but use the selected item (or close the wheel if it's open) if tapped
 * Fate 29-11-21,
 * Changed the UI interactablity in line with designer's needs: item wheel closes when let go.
 */
public class SelectedItemButton : MonoBehaviour
{
    [SerializeField] private float open_menu_time = 0.5f;
    [SerializeField] private ItemSelectionWheel itemWheel;
    [SerializeField] private ItemSelectionWheel potionWheel;

    private float timer = 0.0f;
    private bool runTimer = false;

    private void Update()
    {
        if (timer > open_menu_time)
        {
            ShowItems(true);
          
            if (itemWheel.IsItemWheelSelected()) itemWheel.UseItem();
            if (potionWheel.IsItemWheelSelected()) potionWheel.UseItem();
        }
    }
    private void FixedUpdate()
    {
        if (runTimer) { timer += Time.deltaTime; }
    }
    public void StartTimer() //Used for button Pointer Enter event
    {  
        runTimer = true;
    }
    public void ResetTimer() //Used for button Pointer Exit event
    {   
        runTimer = false;
        timer = 0f;

        ShowItems(false);
    }
    private void ShowItems(bool showItem)
    {
        if (showItem)
        {
            itemWheel.itemWheelSelected = true;
            potionWheel.itemWheelSelected = true;
        }
        else
        {
            itemWheel.itemWheelSelected = false;
            potionWheel.itemWheelSelected = false;
        }
    }

}