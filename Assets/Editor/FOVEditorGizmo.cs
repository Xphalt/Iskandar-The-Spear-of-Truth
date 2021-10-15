////Bernardo Mendes
////AI Programming Dept.
////Latest Rev: 29/09/2021
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(PlayerDetection))]
//public class FOVEditorGizmo : Editor
//{

//    private void OnSceneGUI()
//    {
//        PlayerDetection detection = (PlayerDetection)target;
//        Handles.color = Color.green;
//        Handles.DrawWireArc(detection.transform.position, Vector3.up, Vector3.forward, 360, detection.detectionRadius);
//        Vector3 viewAngleA = detection.DirFromAngle(-detection.viewAngle / 2, false);
//        Vector3 viewAngleB = detection.DirFromAngle(detection.viewAngle / 2, false);

//        Handles.DrawLine(detection.transform.position, detection.transform.position + viewAngleA * detection.detectionRadius);
//        Handles.DrawLine(detection.transform.position, detection.transform.position + viewAngleB * detection.detectionRadius);

//        Handles.color = Color.yellow;
//        Handles.DrawLine(detection.transform.position, detection.GetCurTarget().position);
    
//    }
//}
