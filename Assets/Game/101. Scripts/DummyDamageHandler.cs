
using UnityEngine;

class DummyDamageHandler : DamageHandler
{
    public override void DamageHandle(DamageData damageData)
    {
        Debug.Log($"앗 너무 아파요 ㅠㅠ {damageData}");
    }
}