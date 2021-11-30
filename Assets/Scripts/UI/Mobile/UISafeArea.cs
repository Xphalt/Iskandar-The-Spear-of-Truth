using UnityEngine;
/*______________________________________________________________________
 * This script moves the mobile UI to the visible parts of the screen.
 * This prevents some UI from being hidden. Contact Fate for any issues.
 * _____________________________________________________________________*/

[RequireComponent(typeof(RectTransform))]
public class UISafeArea : MonoBehaviour
{
    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect safeArea = Screen.safeArea;
        Vector3 anchorMin = safeArea.position;
        Vector3 anchorMax = anchorMin + (Vector3)safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
