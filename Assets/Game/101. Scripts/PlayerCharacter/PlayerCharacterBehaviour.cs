using UnityEngine;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerCharacterBehaviour : MonoBehaviour
{
    #region 인스펙터 변수
    // 코드에서 이 변수에 직접 접근하는 것은 지양하세요!
    [SerializeField] int hp = 100;

    [SerializeField] int stamina = 100;

    [Range(0, 10)]
    [SerializeField] float speed = 1;

    [SerializeField] float noDamageTimeByHit;

    [SerializeField] int maxSwitchingPoints = 100;

    [SerializeField] int switchingPointsMinRequirements;

    [SerializeField] int switchingCoolTime;

    [SerializeField] int staminaRecoveryDelay;

    [SerializeField] int staminaRecoveryPerSecond;

    [SerializeField] int sprintStaminaConsumption;

    [SerializeField] int evationStamianConsumption;

    [SerializeField] int parryingSwitchingPointProduction;
    #endregion

    public int Hp
    {
        get => this.hp;
        set => this.hp = value;
    }
    public int Stamina
    {
        get => this.stamina;
        set => this.stamina = value;
    }
    public float Speed
    {
        get => this.speed;
        set => this.speed = value;
    }
    public float NoDamageTimeByHit => this.noDamageTimeByHit;
    public int MaxSwitchingPoints => this.maxSwitchingPoints;
    public int SwitchingPointsMinRequirements => this.switchingPointsMinRequirements;
    public int SwitchingCoolTime => this.switchingCoolTime;
    public int StaminaRecoveryDelay => this.staminaRecoveryDelay;
    public int StaminaRecoveryPerSecond => this.staminaRecoveryPerSecond;
    public int SprintStaminaConsumption => this.sprintStaminaConsumption;
    public int EvationStamianConsumption => this.evationStamianConsumption;
    public int ParryingSwitchingPointProduction => this.parryingSwitchingPointProduction;


    Transform thisTransform;
    PlayerCharacterController characterControl;
    Animator animator;

    Transform mainCameraTransform;

    public void Awake()
    {
        thisTransform = transform;
        characterControl = GetComponent<PlayerCharacterController>();
        animator = GetComponent<Animator>();

        mainCameraTransform = Camera.main.transform;
    }

    public void FixedUpdate()
    {
        var moveVelocity = characterControl.MoveInput;

        var moveMagnitude = moveVelocity.magnitude;
        if (moveMagnitude > 0.1f)
        {
            LookAt(moveVelocity);
            animator.SetFloat("ySpeed", moveMagnitude);
        }
        else
        {
            animator.SetFloat("ySpeed", 0);
        }
    }

    void LookAt(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg - 90;
        // 카메라가 바라보는 방향
        thisTransform.rotation = Quaternion.Euler(0, mainCameraTransform.rotation.eulerAngles.y + angle, 0);
    }
}