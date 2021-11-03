using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Skeleton))]
public class SkeletonAgroRangeEditor : Editor
{

	void OnSceneGUI()
	{
		Skeleton agroRange = (Skeleton)target;
		
		Handles.color = Color.yellow;
		Handles.DrawWireArc(agroRange.transform.position, Vector3.up, Vector3.forward, 360, agroRange.skeletonAgroRadius);
		Vector3 viewAngleA = agroRange.DirFromAngle(-agroRange.agroSupportAngle / 2, false);
		Vector3 viewAngleB = agroRange.DirFromAngle(agroRange.agroSupportAngle / 2, false);
		Handles.DrawLine(agroRange.transform.position, agroRange.transform.position + viewAngleA * agroRange.skeletonAgroRadius);
		Handles.DrawLine(agroRange.transform.position, agroRange.transform.position + viewAngleB * agroRange.skeletonAgroRadius);
	}

}