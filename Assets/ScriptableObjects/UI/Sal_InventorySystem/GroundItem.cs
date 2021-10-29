using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject_Sal itemobj;

    public delegate void Spawn(GameObject obj);
    public Spawn spawn;

    void Start()
    {
        if(spawn != null)
        {
            OnBeforeSerialize(); //Doesn't get called automatically for some reason
            spawn.Invoke(gameObject); //Spawn movement
            StartCoroutine(StopForce());
        }
    }


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

    IEnumerator StopForce()
    {
        yield return new WaitForSeconds(1.0f);
        //Stop drop
        Rigidbody objRgdBody = GetComponentInChildren<Rigidbody>();
        objRgdBody.velocity = Vector3.zero;
        objRgdBody.angularVelocity = Vector3.zero; 
    }
}
