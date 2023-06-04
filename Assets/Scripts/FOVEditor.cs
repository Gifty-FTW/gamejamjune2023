using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FOVEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, viewAngleB, fow.viewAngle, fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
    }
}
