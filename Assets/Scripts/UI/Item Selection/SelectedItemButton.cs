/*
 * Dominique 11-11-2021,
 * Add functionality to open/close the ItemSelectionwheel if held but use the selected item (or close the wheel if it's open) if tapped
 */

using UnityEngine;

public class SelectedItemButton : MonoBehaviour
{
    [SerializeField] private float open_menu_time = 0.5f;
    [SerializeField] private ItemSelectionWheel itemWheel;
    [SerializeField] private ItemSelectionWheel potionWheel;

    private float timer = 0.0f;
    private bool runTimer = false;
    private bool isHolding;

    private void Update()
    {
        if (runTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public void Holding()
    {
        runTimer = true;
        isHolding = true;
    }

    public void LetGo()
    {
        runTimer = false;

        ///on icon hold, start timer
        ///if timer > 0 
        ///     open tool menu
        ///     if finger has raycase on item
        ///         select item
        ///else if icon release
        ///     reset timer
        

        // If we've held the button down or the menu is already open then open/close it respectively
        if (timer > open_menu_time || itemWheel.IsItemWheelSelected())
        {
            itemWheel.ToggleItemSelectionWheel();
            potionWheel.ToggleItemSelectionWheel();
            print("words");
        }
        else // Use the selected item
        {
            itemWheel.UseItem();
        }
        timer = 0.0f;
    }
}
