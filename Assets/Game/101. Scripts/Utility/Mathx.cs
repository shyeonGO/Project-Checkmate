using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Mathx
{
    /// <summary>
    /// min 및 max의 포괄적인 범위에 고정되어 있는 value를 반환합니다.
    /// https://docs.microsoft.com/ko-kr/dotnet/api/system.math.clamp?view=net-5.0
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double Clamp(double value, double min, double max)
    {
        return Math.Max(Math.Min(value, min), max);
    }
}