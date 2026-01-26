using JLGA.Unity.VisualNovel.Listener;
using System.Collections.Generic;
using UnityEditor;

namespace JLGA.Unity.VisualNovel.VNScript.Listener.Editors
{
    [CustomEditor(typeof(VNFlags))]
    public class VNFlagsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VNFlags vnFlags = (VNFlags)target;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Flags");

            if (vnFlags.Flags != null)
            {
                if (vnFlags.Flags.Count == 0)
                {
                    EditorGUILayout.LabelField("Empty");
                }
                foreach (KeyValuePair<string, string> flagValue in vnFlags.Flags)
                {
                    EditorGUILayout.LabelField(flagValue.Key, flagValue.Value);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Null");
            }

            EditorGUILayout.EndVertical();
        }
    }
}
