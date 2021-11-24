using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils : MonoBehaviour
{
    /// <summary>
    /// Converts Screen Position to World Position
    /// </summary>
    /// <param name="camera">Main Camera</param>
    /// <param name="position">Screen Position</param>
    /// <returns>World Position - Vector3</returns>
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        return ray.origin;
    }

    /// <summary>
    /// Checks if there is any UI element where the player pressed
    /// </summary>
    /// <param name="touchPosition">Touch Position</param>
    /// <returns>Returns true if the position is above any UI element</returns>
    public static bool DiscardSwipe(Vector2 touchPosition)
    {
        PointerEventData touch = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(touch, hits);
        return (hits.Count > 0);
    }
}
