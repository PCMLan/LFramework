using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectExtension
{
    public static void FastSetActive(this Transform trans, bool isActive)
    {
        if (trans != null)
        {
            trans.gameObject.FastSetActive(isActive);
        }
    }

    public static void FastSetActive(this GameObject obj, bool isActive)
    {
        if (obj != null && obj.activeSelf != isActive)
        {
            obj.SetActive(isActive);
        }
    }

    public static void FastSetActive(this Button obj, bool isActive)
    {
        if (obj != null && obj.gameObject.activeSelf != isActive)
        {
            obj.gameObject.SetActive(isActive);
        }
    }

    public static void FastSetActive(this Image obj, bool isActive)
    {
        if (obj != null && obj.gameObject.activeSelf != isActive)
        {
            obj.gameObject.SetActive(isActive);
        }
    }

    public static void FastSetActive(this Text obj, bool isActive)
    {
        if (obj != null && obj.gameObject.activeSelf != isActive)
        {
            obj.gameObject.SetActive(isActive);
        }
    }

    public static T FastSetActive<T>(this T obj, bool isActive) where T : Component
    {
        if (obj != null && obj.gameObject.activeSelf != isActive)
        {
            obj.gameObject.SetActive(isActive);
        }
        return obj;
    }

    public static T GetOrAddComponent<T>(this GameObject selfObj) where T : Component
    {
        var component = selfObj.GetComponent<T>();
        if (component == null)
        {
            return selfObj.AddComponent<T>();
        }
        return component;
    }
}
