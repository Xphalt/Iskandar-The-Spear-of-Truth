using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/*
 * Created by Mattie Hilton - 03/10/2021 
 */
public class SettingsOptions : MonoBehaviour
{
    public Dropdown qualityDropDown;
    public string qualitySettingsStringTable = "QualitySettings";

    private LocaleIdentifier current_locale;
    private bool done_first_setup = false;

    // Dominique 08-10-2021, When the pause button is pressed the pause menu opens and the quality settings are populated with options taken from the current locale
    public void LocaleChange()
    {
        LocaleIdentifier new_locale = LocalizationSettings.SelectedLocale.Identifier;

        if (!done_first_setup || (new_locale != current_locale))
        {
            qualityDropDown.ClearOptions();

            if (new_locale.Code.Equals("en"))
            {
                foreach (var name in QualitySettings.names)
                {
                    qualityDropDown.options.Add(new Dropdown.OptionData(name));
                }
            }
            // Use the English quality names to get the codes from the French part of the QualitySettings table
            else if (new_locale.Code.Equals("fr"))
            {
                LocalizedString localisedString = new LocalizedString();
                localisedString.TableReference = qualitySettingsStringTable;
                foreach (var name in QualitySettings.names)
                {
                    localisedString.TableEntryReference = name;
                    string french_name = localisedString.GetLocalizedString();
                    qualityDropDown.options.Add(new Dropdown.OptionData(french_name));
                }
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