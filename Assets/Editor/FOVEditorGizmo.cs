//Bernardo Mendes
//AI Programming Dept.
//Latest Rev: 29/09/2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyBase))]
public class FOVEditorGizmo : Editor
{

    private void OnSceneGUI()
    {
        EnemyBase enemy = (EnemyBase)target;
        Handles.color = Color.green;
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, Vector3.forward, 360, enemy.chaseRadius);
        Vector3 viewAngleA = enemy.DirFromAngle(-enemy.viewAngle/2, false);
        Vector3 viewAngleB = enemy.DirFromAngle(enemy.viewAngle / 2, false);

        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngleA * enemy.chaseRadius);
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngleB * enemy.chaseRadius);

        Handles.color = Color.yellow;
        foreach(Transform visibleTarget in enemy.visibleTargets)
        {
            Handles.DrawLine(enemy.transform.position, visibleTarget.position);
        }
    
    }
}
