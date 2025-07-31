using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class DuplicateComponentCleaner : EditorWindow
{
    // 可供勾选的所有组件类型（你可以扩展这个列表）
    private static readonly Type[] AllSupportedTypes = new Type[]
    {
        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CapsuleCollider),
        typeof(AudioSource),
        typeof(Rigidbody),
        typeof(Light),
        typeof(Camera),
        // 添加更多你需要支持的组件类型
    };

    private Dictionary<Type, bool> typeToggles = new Dictionary<Type, bool>();

    [MenuItem("Tools/Clean Duplicate Components")]
    public static void ShowWindow()
    {
        GetWindow<DuplicateComponentCleaner>("Duplicate Cleaner");
    }

    private void OnEnable()
    {
        // 初始化开关状态（默认全选）
        foreach (var type in AllSupportedTypes)
        {
            if (!typeToggles.ContainsKey(type))
                typeToggles[type] = true;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("选择要检查重复的组件类型", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        foreach (var type in AllSupportedTypes)
        {
            typeToggles[type] = EditorGUILayout.ToggleLeft(type.Name, typeToggles[type]);
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        if (GUILayout.Button("清除选中物体及其子物体的重复组件"))
        {
            var whiteListedTypes = GetSelectedTypes();
            int totalRemoved = 0;
            var selectedObjects = Selection.gameObjects;
            foreach (var go in selectedObjects)
            {
                totalRemoved += CleanDuplicatesRecursive(go, whiteListedTypes);
            }

            Debug.Log($"清理完成，总共删除了 {totalRemoved} 个重复组件。");
        }
    }

    private List<Type> GetSelectedTypes()
    {
        List<Type> selected = new List<Type>();
        foreach (var kvp in typeToggles)
        {
            if (kvp.Value)
                selected.Add(kvp.Key);
        }
        return selected;
    }

    private int CleanDuplicatesRecursive(GameObject go, List<Type> whiteListedTypes)
    {
        int removed = CleanDuplicatesOnGameObject(go, whiteListedTypes);

        foreach (Transform child in go.transform)
        {
            removed += CleanDuplicatesRecursive(child.gameObject, whiteListedTypes);
        }

        return removed;
    }

    private int CleanDuplicatesOnGameObject(GameObject go, List<Type> whiteListedTypes)
    {
        Component[] allComponents = go.GetComponents<Component>();
        Dictionary<Type, bool> seenTypes = new Dictionary<Type, bool>();
        int removedCount = 0;

        for (int i = allComponents.Length - 1; i >= 0; i--)
        {
            var comp = allComponents[i];
            if (comp == null) continue;

            Type type = comp.GetType();

            if (!whiteListedTypes.Contains(type))
                continue;

            if (seenTypes.ContainsKey(type))
            {
                Undo.DestroyObjectImmediate(comp);
                removedCount++;
            }
            else
            {
                seenTypes[type] = true;
            }
        }

        return removedCount;
    }
}
