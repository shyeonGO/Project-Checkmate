
using System.Runtime.CompilerServices;
using UnityEngine;

public static class UnityObjectExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToReferenceNull<T>(this T unityObject) where T : UnityEngine.Object
    {
        return unityObject != null ? unityObject : null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull(this UnityEngine.Object unityObject)
    {
        return UnityEngine.Object.Equals(unityObject, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsReferenceNull(this UnityEngine.Object unityObject)
    {
        return ReferenceEquals(unityObject, null);
    }
}