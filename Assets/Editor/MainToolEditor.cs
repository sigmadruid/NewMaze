using UnityEngine;
using UnityEditor;

using System;
using System.Collections;

[CustomEditor(typeof(Main))]
public class MainToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if(GUILayout.Button("Clear Saving"))
        {
            try
            {
                GameLogic.RecordMediator.DeleteRecord();
                Debug.Log("Delete record successfully!");
            }
            catch(Exception e)
            {
                Debug.LogError(e.Data);
            }
        }
    }
}
