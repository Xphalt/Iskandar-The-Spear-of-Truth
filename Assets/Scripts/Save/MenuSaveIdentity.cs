using UnityEngine;


public class MenuSaveIdentity : MonoBehaviour
{
    public PlayerNameManager pnm;

    public int id;

    public void UpdateCurrentSaveID()
    {
        pnm.SetSaveNum(id);
    }
}
