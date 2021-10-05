
/* 
 * Dominique, 30-09-2021
 * Functions to use shaders
 */

using UnityEngine;

public class ShaderHandler : MonoBehaviour
{
    public static ShaderHandler instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Dominique, Had to delete duplicate ShaderHandler");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /* 
     * Dominique, 30-09-2021
     * Pass in game object and desired colour to make outline appear
     * Use Color.clear to make it disappear
     */
    public void SetOutlineColor(GameObject gameObject, Color color)
    {
        Outline outline;
        bool gotOutline = gameObject.TryGetComponent<Outline>(out outline);

        if (gotOutline)
        {
            outline.OutlineColor = color;
        }
    }
}
