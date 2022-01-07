using UnityEditor;
using UnityEngine;
using System;

namespace Assets.Scripts.Utility
{
    public class MaterialConverter : ScriptableObject
    {
        [MenuItem("Tools/Fix materials")]
        static void Do()
        {
            var standardShader = Shader.Find("Standard");
            var materialsGuids = AssetDatabase.FindAssets($"t:{nameof(Material)}");
            EditorUtility.DisplayProgressBar("Fixing materials", "", 0);
            var counter = 0;
            foreach (var materialGuid in materialsGuids)
            {
                var materialPath = AssetDatabase.GUIDToAssetPath(materialGuid);
                var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                EditorUtility.DisplayProgressBar("Fixing materials", material.name, counter++ / (float)materialsGuids.Length);
                try
                {
                    var shader = material.shader;
                    if (shader.name == "Hidden/InternalErrorShader")
                    {
                        Debug.Log(material.name, material);
                        material.shader = standardShader;
                        EditorUtility.SetDirty(material);
                        AssetDatabase.SaveAssets();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString(), material);
                    break;
                }
                finally
                {
                    EditorUtility.DisplayProgressBar("Fixing materials", "", counter / (float)materialsGuids.Length);
                }
            }
            EditorUtility.ClearProgressBar();
        }
    }
}