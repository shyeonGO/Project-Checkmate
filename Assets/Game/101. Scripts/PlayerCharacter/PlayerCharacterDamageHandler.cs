
using UnityEngine;

class PlayerCharacterDamageHandler : DamageHandler
{
    [SerializeField] PlayerCharacterBehaviour behaviour;

    private void Awake()
    {
        if (behaviour == null)
        {
            behaviour = GetComponent<PlayerCharacterBehaviour>();
        }
    }

    public override void DamageHandle(DamageData damageData)
    {
        if (!behaviour.IsNoDamage)
        {
            Debug.Log($"플레이어 데미지 핸들: {damageData}");
            behaviour.Status.Hp -= damageData.Damage;
            behaviour.DoImpact();
        }
        else
        {
            Debug.Log($"플레이어 무적, 데미지 수신: {damageData}");
        }
    }
}