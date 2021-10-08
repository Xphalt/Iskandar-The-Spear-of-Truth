using UnityEngine;
using UnityEngine.Localization;

/*
 *  Dominique Russell 08-10-2021
 *  Use to access localisation tables in code easily
 *  tableReference and entryReference can be passed in through the inspector and the string can be collected with a call to GetLocalisedString()
 */

[System.Serializable]
public struct LocalisationTableReference
{
    public string tableReference;
    public string entryReference;

    public string GetLocalisedString()
    {
        LocalizedString localisedString = new LocalizedString();
        localisedString.TableReference = tableReference;
        localisedString.TableEntryReference = entryReference;
        return localisedString.GetLocalizedString();
    }
}