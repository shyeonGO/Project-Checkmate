using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct DamageData
{
    /// <summary>
    /// 데미지를 넘겨주는 측의 트리거입니다.
    /// </summary>
    /// <remarks>
    /// 데미지 트리거의 멤버에 접근하려면 if (damageTrigger is ChildDamageTrigger childDamageTrigger) 같은 형태로 타입 체크 및 캐스팅하여 사용하세요.
    /// </remarks>
    public DamageTrigger Trigger;
    /// <summary>
    /// 데미지
    /// </summary>
    public double Damage;
    /// <summary>
    /// 그로기 포인트
    /// </summary>
    public double GroggyPoint;


    public override string ToString() => $"{{Trigger: {(Trigger == null ? "null" : $"\"{Trigger}\"")}, Damage: {Damage}, GroggyPoint: {GroggyPoint}}}";
}