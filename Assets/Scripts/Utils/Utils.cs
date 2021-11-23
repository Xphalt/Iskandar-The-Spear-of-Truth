using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        return ray.origin;
    }
}
