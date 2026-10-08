using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;

//[CustomEditor(typeof(MCObj), true)]
public class UIBaseInspector : Editor
{
    private static string m_SearchedMemberName = "";

    public override void OnInspectorGUI()
    {
        //MCBaseLayer window = target as MCBaseLayer;

        m_SearchedMemberName = EditorGUILayout.TextField("Quick Search", m_SearchedMemberName);
        EditorGUILayout.Separator();

        if (m_SearchedMemberName.Length > 0)
        {
            System.Type type = target.GetType();
            SerializedObject serObj = new SerializedObject(target);
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            IEnumerable<FieldInfo> memberInfos = type.GetFields(flags | BindingFlags.DeclaredOnly);
            IEnumerable<FieldInfo> baseMemberInfos = type.BaseType.GetFields(flags | BindingFlags.DeclaredOnly);
            memberInfos = memberInfos.Concat(baseMemberInfos);
            foreach (FieldInfo field in memberInfos)
            {
                System.Type rt = field.ReflectedType;

                if (rt != typeof(Object) && rt != typeof(Component) && rt != typeof(MonoBehaviour) && rt != typeof(Behaviour))
                {
                    bool matched = false;
                    object value = field.GetValue(target);
                    IEnumerable enumerable = value as IEnumerable;
                    if (enumerable != null)
                    {
                        foreach (object element in enumerable)
                        {
                            Object obj = element as Object;
                            if (obj && IsMatchSearch(obj.name, m_SearchedMemberName))
                            {
                                matched = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Object obj = value as Object;
                        if (obj && IsMatchSearch(obj.name, m_SearchedMemberName))
                        {
                            matched = true;
                        }
                    }

                    if (IsMatchSearch(field.Name, m_SearchedMemberName))
                    {
                        matched = true;
                    }

                    if (matched)
                    {
                        GUI.SetNextControlName(field.Name);
                        var fp = serObj.FindProperty(field.Name);
                        if (fp != null)
                            EditorGUILayout.PropertyField(fp, true);
                    }
                }
            }
            serObj.ApplyModifiedProperties();
        }
        else
        {
            base.OnInspectorGUI();
        }
    }

    bool IsMatchSearch(string src, string key)
    {
        return src.ToLower().Contains(key.ToLower());
    }
}
