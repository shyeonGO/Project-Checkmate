
using System.Runtime.CompilerServices;
using UnityEngine;

public static class UnityObjectExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T DoNotNull<T>(this T unityObject) where T : UnityEngine.Object
    {
        return unityObject != null ? unityObject : null;
    }
}