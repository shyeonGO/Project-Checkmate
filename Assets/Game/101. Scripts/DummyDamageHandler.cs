
using UnityEngine;

class DummyDamageHandler : DamageHandler
{
    public override void DamageHandle(int damage)
    {
        Debug.Log($"앗 너무 아파요 ㅠㅠ {damage}");
    }
}