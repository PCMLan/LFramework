using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ClassExtention
{
    public static bool IsNull<T>(this T c) where T : class
    {
        return c == null;
    }

    public static bool IsNotNull<T>(this T c) where T : class
    {
        return c != null;
    }
}