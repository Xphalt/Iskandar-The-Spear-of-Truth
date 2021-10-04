using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 * Created by Mattie Hilton - 03/10/2021 
 */
public class SettingsOptions : MonoBehaviour
{
    public Dropdown qualityDropDown;

    void Start()
    {
        qualityDropDown.ClearOptions();

        foreach (var name in QualitySettings.names)
        {
            qualityDropDown.options.Add(new Dropdown.OptionData(name));
        }

        SetQuality(QualitySettings.GetQualityLevel());
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropDown.value = qualityIndex;
        qualityDropDown.RefreshShownValue();
    }
}