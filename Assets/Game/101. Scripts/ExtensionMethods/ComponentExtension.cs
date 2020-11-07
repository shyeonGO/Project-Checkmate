
using UnityEngine;

public static class ComponentExtension
{
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
}