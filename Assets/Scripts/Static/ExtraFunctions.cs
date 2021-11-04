//////////////////////////////////////////////////////////////////////////////////
//CREATED BY DANIEL, CONTAINS SOME POTENTIALLY USELFUL MATHS/TRANSFORM FUNCTIONS//
//////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtraFunctions
{
    public static Vector2 To2D(this Vector3 self)
    {
        return new Vector2(self.x, self.y);
    }

    public static void RotateToVector2(this Transform self, Vector2 target)
    {
        if (target.magnitude == 0) return;
        float newRotation;
        if (target.y != 0) newRotation = -Mathf.Rad2Deg * Mathf.Atan(target.x / target.y) - ((target.y < 0) ? 180 : 0);
        else newRotation = 90 * ((target.x < 0) ? 1 : -1);
        self.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    public static void RotateAroundPoint(this Transform self, Vector3 targetPoint, Vector3 axis, float angle)
    {
        Vector3 direction = self.position - targetPoint;
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        self.position = targetPoint + rotation * direction;
        self.Rotate(axis * angle);
    }

    public static Vector2 RandomVector2(float scale=1)
    {
        return new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * scale;
    }

    public static Vector3 RandomVector3(float scale=1)
    {
        return new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f).normalized * scale;
    }

    public static float RandomVariance(float variance)
    {
        return Random.Range(1 - variance, 1 + variance);
    }

    public static float GetDistance(this Transform self, Transform target)
    {
        return (target.position - self.position).magnitude;
    }

    public static float GetDistance(this Vector3 self, Vector3 target)
    {
        return (target - self).magnitude;
    }

    public static bool OnScreen(this Transform self)
    {
        Vector2 cameraMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 cameraMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        return (self.position.x < cameraMax.x && self.position.x > cameraMin.x && self.position.y < cameraMax.y && self.position.y > cameraMin.y);
    }

    public static bool OnScreen(this Vector2 self)
    {
        Vector2 cameraMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 cameraMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        return (self.x < cameraMax.x && self.x > cameraMin.x && self.y < cameraMax.y && self.y > cameraMin.y);
    }

    public static void CopyRectTransform(this RectTransform self, RectTransform target)
    {
        self.anchorMin = target.anchorMin;
        self.anchorMax = target.anchorMax;
        self.anchoredPosition = target.anchoredPosition;
        self.sizeDelta = target.sizeDelta;
    }

    public static int RoundToNearest(float start, int target)
    {
        return Mathf.RoundToInt(start / target) * target;
    }

    public static float RoundToDP(float start, int DP)
    {
        return Mathf.Round(start * Mathf.Pow(10, DP)) / Mathf.Pow(10, DP);
    }

    public static float ToDP(this float self, int DP)
    {
        return Mathf.Round(self * Mathf.Pow(10, DP)) / Mathf.Pow(10, DP);
    }

    public static int ToNearest(this float self, int target)
    {
        return Mathf.RoundToInt(self / target) * target;
    }

    public static int Sum(this List<int> self)
    {
        int sum = 0;
        foreach (int i in self) sum += i;
        return sum;
    }

    public static float Sum(this List<float> self)
    {
        float sum = 0;
        foreach (float f in self) sum += f;
        return sum;
    }

    public static float Average(this List<float> self)
    {
        return self.Sum() / self.Count;
    }

    public static float Average(this List<int> self)
    {
        return self.Sum() / self.Count;
    }
}
