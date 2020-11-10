
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ComponentExtension
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareTags(this Component component, params string[] tags)
    {
        if (tags.Length != 0)
        {
            foreach (var tag in tags)
            {
                if (component.CompareTag(tag))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 컴포넌트를 찾거나 없으면 새로 추가합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetComponentOrNew<T>(this Component component, bool showPerformanceWarning = true) where T : Component
    {
        if (component.TryGetComponent<T>(out var foundComponent))
        {
            return foundComponent;
        }
        else
        {
            if (showPerformanceWarning)
                Debug.LogWarning($"GetComponentOrNew<{typeof(T)}>성능 경고: {typeof(T)}컴포넌트가 할당되지 않아 새 컴포넌트가 할당되었습니다.");
            return component.gameObject.AddComponent<T>();
        }
    }
}