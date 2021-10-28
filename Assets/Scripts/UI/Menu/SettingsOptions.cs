using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

/*
 * Created by Mattie Hilton - 03/10/2021 
 */
public class SettingsOptions : MonoBehaviour
{
    public TMP_Dropdown qualityDropDown;
    public string qualitySettingsStringTable = "QualitySettings";

    private LocaleIdentifier current_locale;
    private bool done_first_setup = false;

    private void Start()
    {
        LocaleChange();
    }

    // Dominique 08-10-2021, When the pause button is pressed the pause menu opens and the quality settings are populated with options taken from the current locale
    public void LocaleChange()
    {
        LocaleIdentifier new_locale = LocalizationSettings.SelectedLocale.Identifier;

        if (!done_first_setup || (new_locale != current_locale))
        {
            qualityDropDown.ClearOptions();

            LocalizedString localisedString = new LocalizedString();
            localisedString.TableReference = qualitySettingsStringTable;
            foreach (var name in QualitySettings.names)
            {
                localisedString.TableEntryReference = name;
                string localised_name = localisedString.GetLocalizedString();
                qualityDropDown.options.Add(new TMP_Dropdown.OptionData(localised_name));
            }

            SetQuality(QualitySettings.GetQualityLevel());

            current_locale = new_locale;
            if (!done_first_setup) done_first_setup = true;
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropDown.value = qualityIndex;
        qualityDropDown.RefreshShownValue();
    }
}