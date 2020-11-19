
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

    public override void DamageHandle(double damage)
    {
        if (!behaviour.IsNoDamage)
        {
            Debug.Log($"플레이어 데미지 핸들: {damage}");
            behaviour.Status.Hp -= damage;
        }
        else
        {
            Debug.Log($"플레이어 무적, 데미지 수신: {damage}");
        }
    }
}