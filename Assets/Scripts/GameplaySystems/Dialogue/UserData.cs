using UnityEngine;

[CreateAssetMenu(fileName = "New UserData", menuName = "UserData")]
[System.Serializable]
public class UserData : ScriptableObject
{
    public string playerName;

    public static new UserData CreateInstance(string name)
    {
        UserData u = CreateInstance<UserData>();

        // Then directly set the values
        u.playerName = name;

        return u;
    }
}