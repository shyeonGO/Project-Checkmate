
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TransformExtension
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Initialize(this Transform transform)
    {
        transform.localPosition = default;
        transform.localRotation = default;
        transform.localScale = Vector3.one;
    }
}