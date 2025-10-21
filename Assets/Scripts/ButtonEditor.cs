using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestScript))]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TestScript script = (TestScript)target;
        if (GUILayout.Button("Test"))
        {
            script.Force();
        }
    }
}
