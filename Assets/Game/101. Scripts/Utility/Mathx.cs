using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public static class Mathx
{
    /// <summary>
    /// min 및 max의 포괄적인 범위에 고정되어 있는 value를 반환합니다.
    /// </summary>
    /// <remarks>
    /// 레퍼런스: https://docs.microsoft.com/ko-kr/dotnet/api/system.math.clamp?view=net-5.0
    /// 소스코드: https://github.com/dotnet/runtime/blob/3642deea8235ce8316db8c37cc3e8f4ad5b15380/src/libraries/System.Private.CoreLib/src/System/Math.cs
    /// </remarks>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Clamp(double value, double min, double max)
    {
        if (min > max)
        {
            throw new ArgumentException("min이 max보다 큽니다.");
        }

        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }

        return value;
    }

    /// <summary>
    /// min 및 max의 포괄적인 범위에 고정되어 있는 value를 반환합니다.
    /// </summary>
    /// <remarks>
    /// 레퍼런스: https://docs.microsoft.com/ko-kr/dotnet/api/system.math.clamp?view=net-5.0
    /// 소스코드: https://github.com/dotnet/runtime/blob/3642deea8235ce8316db8c37cc3e8f4ad5b15380/src/libraries/System.Private.CoreLib/src/System/Math.cs
    /// </remarks>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException("min이 max보다 큽니다.");
        }

        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }

        return value;
    }

    public static void TimeToZero(ref float time, in float deltaTime)
    {
        time -= deltaTime;
        if (time < 0)
        {
            time = 0;
        }
    }
}