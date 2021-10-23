using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject_Sal itemobj;

    public void OnAfterDeserialize()
    { }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (itemobj)
        {
            GetComponentInChildren<MeshFilter>().mesh = itemobj.model;
            //This let the editor know that something has changed on that object 
            EditorUtility.SetDirty(GetComponentInChildren<MeshFilter>());
        }
#else
        if (itemobj)
        {
            GetComponentInChildren<MeshFilter>().mesh = itemobj.model;
        }
#endif
    }

    public void SetItem(ItemObject_Sal obj)
    {
        itemobj = obj;
    }
}
