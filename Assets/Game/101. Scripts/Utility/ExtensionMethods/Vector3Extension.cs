using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 ToXZY(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.z, vector.y);
    }

    public static Vector2 ToXZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}
