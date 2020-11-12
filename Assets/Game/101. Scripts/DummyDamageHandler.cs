
using UnityEngine;

class DummyDamageHandler : DamageHandler
{
    public override void DamageHandle(double damage)
    {
        Debug.Log($"앗 너무 아파요 ㅠㅠ {damage}");
    }
}