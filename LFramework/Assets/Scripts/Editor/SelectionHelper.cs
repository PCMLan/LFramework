using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Lan.SelectionHelper
{
    public static class SelectionHelper
    {
        [InitializeOnLoadMethod]
        private static void Start()
        {
            //在Hierarchy面板按鼠标中键相当于开关GameObject
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;

            //在Project面板按鼠标中键相当于Show In Explorer
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGui;
        }

        private static void ProjectWindowItemOnGui(string guid, Rect selectionRect)
        {
            Event e = Event.current;
            if (selectionRect.Contains(e.mousePosition))
            {
                string strPath = AssetDatabase.GUIDToAssetPath(guid);

                if (e.type == EventType.MouseDown && e.button == 2)
                {
                    if (Path.GetExtension(strPath) == string.Empty) //文件夹
                    {
                        Process.Start(Path.GetFullPath(strPath));
                    }
                    else //文件
                    {
                        Process.Start("explorer.exe", "/select," + Path.GetFullPath(strPath));
                    }
                    e.Use();
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftAlt)
                {
                    Debug.Log(strPath);
                    e.Use();
                }
            }
        }

        private static void HierarchyWindowItemOnGui(int instanceID, Rect selectionRect)
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case KeyCode.Space:
                        ToggleGameObjcetActiveSelf();
                        e.Use();
                        break;
                    case KeyCode.LeftAlt:
                        Selection.gameObjects.ForEach(go => Debug.Log(GetHierarchyPath(go.transform)));
                        e.Use();
                        break;
                }
            }
            else if (e.type == EventType.MouseDown && e.button == 2)
            {
                ToggleGameObjcetActiveSelf();
                e.Use();
            }
        }

        public static void ToggleGameObjcetActiveSelf()
        {
            Undo.RecordObjects(Selection.gameObjects, "Active");
            Selection.gameObjects.ForEach(go =>
            {
                go.SetActive(!go.activeSelf);
            });
        }

        public static T[] ForEach<T>(this T[] selfArray, Action<T> action)
        {
            Array.ForEach(selfArray, action);
            return selfArray;
        }

        /// <summary>
        /// 获取在Hierarchy面板上的路径
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static string GetHierarchyPath(Transform transform)
        {
            StringBuilder sb = new StringBuilder();
            Transform t = transform;
            while (true)
            {
                sb.Insert(0, t.name);
                t = t.parent;
                if (t)
                {
                    sb.Insert(0, "/");
                }
                else
                {
                    return sb.ToString();
                }
            }
        }
    }
}

