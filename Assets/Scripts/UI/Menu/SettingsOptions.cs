using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.UI;
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

    public AudioMixer masterMixer;

    private SaveDataAssistant sda;

    private void Start()
    {
        LocaleChange();
        sda = GameObject.FindObjectOfType<SaveDataAssistant>();
        SetMusicVolume(sda.MusicVol);
        SetSFXVolume(sda.SFXVol);
        SetAmbienceVolume(sda.AmbienceVol);
        transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Slider>().value = sda.MusicVol;
        transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Slider>().value = sda.SFXVol;
        transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<Slider>().value = sda.AmbienceVol;
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
        Debug.Log("Setting Quality to " + QualitySettings.GetQualityLevel() + " " + qualityIndex);
    }

    public void SetMusicVolume(float volume)
    {
        if(volume < -70)
        masterMixer.SetFloat("musicVol", -80);
        else masterMixer.SetFloat("musicVol", volume/2);
        sda.MusicVol = volume;
    }

    public void SetAmbienceVolume(float volume)
    {
        if (volume < -70)
            masterMixer.SetFloat("ambienceVol", -80);
        else masterMixer.SetFloat("ambienceVol", volume / 2);
        sda.AmbienceVol = volume;
    }

    public void SetSFXVolume(float volume)
    {
        if (volume < -70)
            masterMixer.SetFloat("sfxVol", -80);
        else masterMixer.SetFloat("sfxVol", volume/2);
        sda.SFXVol = volume;
    }
}