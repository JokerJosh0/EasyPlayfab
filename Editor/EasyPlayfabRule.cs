#if UNITY_EDITOR
namespace EasyPlayfab.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using PlayFab.PfEditor;
    using PlayFab.PfEditor.EditorModels;
    using PlayFab;
    using PlayFab.ClientModels;

    public class EasyPlayfabRule : EditorWindow
    {
        [MenuItem("EasyPlayfab/Rule", false, 2)]
        private static void Init()
        {
            EasyPlayfabRule s = CreateWindow<EasyPlayfabRule>();
            s.Show();
            s.titleContent = new GUIContent()
            {
                text = "Rule Creation",
                tooltip = "Tutorial on how to create a playfab rule"
            };
        }
        int step = 0;
        float sizeMulti;
        public void OnGUI()
        {
            minSize = new Vector2(870 * sizeMulti, 540 * sizeMulti);
            maxSize = new Vector2(870 * sizeMulti, 540 * sizeMulti);
            if (step == 0)
            {
                GUILayout.BeginVertical("box");
                if (GUILayout.Button("Next Step"))
                {
                    step += 1;
                }
                sizeMulti = EditorGUILayout.Slider("Size Multiplier", sizeMulti, 1, 5);
                GUILayout.EndVertical();
                GUILayout.Label("Step One:");
                GUI.DrawTexture(new Rect(17.44f, 74.7f, 841.3f * sizeMulti, 456.3f * sizeMulti), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/EasyPlayfab/Editor/Tutorial IMG's/Step1.png", typeof(Texture2D)));
            }
            else
            {
                GUILayout.BeginVertical("box");
                if (GUILayout.Button("Last Step"))
                {
                    step -= 1;
                }
                sizeMulti = EditorGUILayout.Slider("Size Multiplier", sizeMulti, 1, 5);
                GUILayout.EndVertical();
                GUILayout.Label("Step Two:");
                GUI.DrawTexture(new Rect(17.44f, 74.7f, 841.3f * sizeMulti, 456.3f * sizeMulti), (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/EasyPlayfab/Editor/Tutorial IMG's/Step2.png", typeof(Texture2D)));
            }
        }
    }
}
#endif