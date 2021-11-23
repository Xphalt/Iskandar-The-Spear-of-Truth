using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public LightSwitch[] switchOrder;
    public List<LightSwitch> switchInputs = new List<LightSwitch>();

    public List<GameObject> activated;
    public List<GameObject> deactivated;

    private bool complete = false;

    public void RegisterSwitch(LightSwitch newSwitch)
    {
        if (complete) return;

        switchInputs.Add(newSwitch);
        if (switchOrder.Length == switchInputs.Count)
        {
            if (CheckComplete()) EndPuzzle();
            else ResetSwitches();
        }
    }

    public bool CheckComplete()
    {
        if (switchInputs.Count < switchOrder.Length) return false;

        for (int s = 0; s < switchInputs.Count; s++)
            if (switchInputs[s] != switchOrder[s]) return false;

        complete = true;
        return true;
    }

    public void EndPuzzle()
    {
        foreach (GameObject go in activated) go.SetActive(true);
        foreach (GameObject go in deactivated) go.SetActive(false);
    }
    
    public void ResetSwitches()
    {
        switchInputs.Clear();
        foreach (LightSwitch ls in switchOrder) ls.TurnOff();
    }
}
