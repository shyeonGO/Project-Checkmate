
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameObjectExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool MatchLayer(this GameObject gameObject, int layerMask)
    {
        return 0 != (layerMask ^ (1 << gameObject.layer));
    }
}