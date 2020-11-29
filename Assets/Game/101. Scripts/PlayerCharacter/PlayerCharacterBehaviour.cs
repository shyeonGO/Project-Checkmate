using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerCharacterInput))]
[RequireComponent(typeof(PlayerCharacterEquipment))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterBehaviour : MonoBehaviour
{
    const int BaseLayerIndex = 0;
    const int WeaponType1LayerIndex = 1;
    const int WeaponType2LayerIndex = 2;
    const int OverrideLayerIndex = 3;

    // 만약 명칭이 바뀌었다면 주석과 변수 모두 변경해줄 필요가 있음.
    // Animator.StringToHash("Attack1")
    const int Attack1Hash = -0x2D200DE;
    // Animator.StringToHash("Attack2")
    const int Attack2Hash = 0x6424AE98;
    // Animator.StringToHash("Attack3")
    const int Attack3Hash = 0x13239E0E;
    // Animator.StringToHash("Attack4")
    const int Attack4Hash = -0x72B8F453;
    // Animator.StringToHash("Attack5")
    const int Attack5Hash = -0x5BFC4C5;

    const int SwitchingAttackHash = 0;
    const int SwitchingLastAttackHash = 0;

    #region 인스펙터 변수
    [SerializeField]
    PlayerCharacterStatus status;
    [SerializeField] float doAttackTime = 1;
    [SerializeField] float doWeaponChangeTime = 1;
    [SerializeField] float doEvadeTime = 1;
    [Header("Ground")]
    [SerializeField] float maxSlope = 45;
    [Header("SmoothTime")]
    [SerializeField] float characterWorldAngleSmoothTime = 0.1f;
    [SerializeField] float moveVelocitySmoothTime = 0.1f;
    [Header("Debug")]
    // 착지 상태
    [SerializeField] bool isGround = false;
    // 가파른 상태
    [SerializeField] bool isSteep = false;
    [SerializeField] bool isLockon = false;
    #endregion


    Transform thisTransform;
    PlayerCharacterInput characterInput;
    PlayerCharacterEquipment characterEquipment;
    Animator thisAnimator;
    AnimatorTriggerManager animatorTriggerManager;
    Rigidbody thisRigidbody;

    Transform mainCameraTransform;

    float characterWorldAngle = 0;
    float characterWorldAngleTarget = 0;
    float characterWorldAngleSmooth = 0;
    Vector2 moveVelocity;
    Vector2 moveVelocitySmooth;

    Vector3 currentClimbDirection;

    // 공격 취소
    bool cancelAttack;
    // 공격 차단
    bool blockAttack;

    bool isEvadingChecked = false;
    [SerializeField] float noDamageTime = 0;
    [SerializeField] int reservedWeaponIndex = 0;
    [SerializeField] float switchingCooltime = 0;

    List<ContactPoint> contactPoints = new List<ContactPoint>(0);

    public Transform Transform => thisTransform;
    public Rigidbody Rigidbody => thisRigidbody;
    public Animator Animator => thisAnimator;
    public PlayerCharacterInput CharacterInput => characterInput;
    public PlayerCharacterStatus Status => status;

    public bool IsAttacking
    {
        get
        {
            var animator = Animator;
            var currentAnimatorState = animator.GetCurrentAnimatorStateInfo(BaseLayerIndex);
            //var animatorTransitionInfo = animator.GetAnimatorTransitionInfo(baseLayerIndex);
            //var nextAnimatorState = animator.GetNextAnimatorStateInfo(baseLayerIndex);

            return currentAnimatorState.IsTag("Attack");
        }
    }

    public void DoAttack()
    {
        animatorTriggerManager.SetTrigger("doAttacking", doAttackTime);
    }

    public bool IsEvading
    {
        get
        {
            var animator = Animator;
            var currentAnimatorState = animator.GetCurrentAnimatorStateInfo(OverrideLayerIndex);
            var nextAnimatorState = animator.GetNextAnimatorStateInfo(OverrideLayerIndex);

            return currentAnimatorState.IsTag("Evade") || nextAnimatorState.IsTag("Evade");
        }
    }

    public void DoImpact()
    {
        Animator.SetTrigger("doImpact");
        Animator.SetTrigger("doBaseCancel");

        //SendMessage("DamageTrigger_EndTrigger");
    }

    public bool IsImpact
    {
        get
        {
            var animator = Animator;
            (var currentAnimatorState, var nextAnimatorState) = animator.GetCurrentAndNextAnimatorStateInfo(OverrideLayerIndex);

            return currentAnimatorState.IsTag("Impact") || nextAnimatorState.IsTag("Impact");
        }
    }

    public bool IsNoDamage
    {
        get
        {
            return IsEvading || noDamageTime > 0;
        }
    }

    public bool IsEvade
    {
        get
        {
            var animator = Animator;
            (var currentAnimatorState, var nextAnimatorState) = animator.GetCurrentAndNextAnimatorStateInfo(OverrideLayerIndex);

            return currentAnimatorState.IsTag("Evade") || nextAnimatorState.IsTag("Evade");
        }
    }

    public bool CanRotate
    {
        get
        {
            var nextStateIsMove = Animator.GetNextAnimatorStateInfo(BaseLayerIndex).IsTag("Move");
            return !IsAttacking &&
                !IsEvading &&
                !IsImpact ||
                nextStateIsMove &&
                !IsEvading;
        }
    }

    public bool BlockAttack
    {
        get => blockAttack || cancelAttack;
        set => blockAttack = value;
    }
    public bool CancelAttack
    {
        get => cancelAttack;
        set => cancelAttack = value;
    }

    public bool IsSwitchingAttackInCurrent => Animator.GetCurrentAnimatorStateInfo(BaseLayerIndex).shortNameHash == Animator.StringToHash("SwitchingAttack");
    public bool IsSwitchingLastAttackInCurrent => Animator.GetCurrentAnimatorStateInfo(BaseLayerIndex).shortNameHash == Animator.StringToHash("SwitchingLastAttack");
    public bool IsLastAttackInCurrent => CurrentAttackCombo == MaxAttackCombo;
    public bool IsLastAttackInNext => NextAttackCombo == MaxAttackCombo;
    public bool IsLastAttackRecently => RecentlyAttackCombo == MaxAttackCombo;

    int recentlyAttackCombo = 0;
    /// <summary>
    /// 최근에 실행된 콤보의 번호를 가져옵니다.
    /// </summary>
    public int RecentlyAttackCombo
    {
        get
        {
            var currentAttackCombo = CurrentAttackCombo;
            if (currentAttackCombo != 0)
                recentlyAttackCombo = currentAttackCombo;
            return recentlyAttackCombo;
        }
    }

    /// <summary>
    /// 현재 실행되는 콤보의 번호를 가져옵니다.
    /// </summary>
    public int CurrentAttackCombo => GetAttackComboFromState(Animator.GetCurrentAnimatorStateInfo(BaseLayerIndex));
    /// <summary>
    /// 트랜지션 중인 다음 공격의 콤보의 번호를 가져옵니다.
    /// </summary>
    public int NextAttackCombo => GetAttackComboFromState(Animator.GetCurrentAnimatorStateInfo(BaseLayerIndex));

    private int GetAttackComboFromState(AnimatorStateInfo animatorState)
    {

        var nameHash = animatorState.shortNameHash;
        switch (nameHash)
        {
            case Attack1Hash:
                return 1;
            case Attack2Hash:
                return 2;
            case Attack3Hash:
                return 3;
            case Attack4Hash:
                return 4;
            case Attack5Hash:
                return 5;
        }
        return 0;
    }

    public int MaxAttackCombo
    {
        get
        {
            return Animator.GetInteger("maxAttackCombo");
        }
        set
        {
            Animator.SetInteger("maxAttackCombo", Mathx.Clamp(value, 0, 5));
        }
    }

    void Awake()
    {
        thisTransform = transform;
        characterInput = GetComponent<PlayerCharacterInput>();
        characterEquipment = GetComponent<PlayerCharacterEquipment>();
        thisAnimator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody>();
        animatorTriggerManager = GetComponent<AnimatorTriggerManager>();

        if (status == null)
            status = this.GetComponentOrNew<PlayerCharacterStatus>();

        mainCameraTransform = Camera.main.transform;

        var forward = thisTransform.forward;

        characterWorldAngle = characterWorldAngleTarget = thisTransform.rotation.eulerAngles.y;
    }

    private void Start()
    {
        characterInput.AttackInputReceived.AddListener(this.AttackInputHandle);
        characterInput.EvadeInputReceived.AddListener(this.EvadeInputHandle);

        var input = CharacterInput;
        var currentWeaponIndex = input.WeaponSwitchInput;
        reservedWeaponIndex = status.CurrentWeaponSlotIndex = currentWeaponIndex;

        characterEquipment.WeaponData = status.GetWeaponSlot(currentWeaponIndex);

        UpdateAnimationSpeed();
    }

    void FixedUpdate()
    {
        GroundUpdate();
    }

    private void Update()
    {
        MoveUpdate();
        AttackUpdate();
        EvadeUpdate();
        WeaponSwitchUpdate();
        LockonUpdate();

        AttackCancelUpdate();

        TimeUpdate();
    }

    Vector3 lastVelocity;

    #region FixedUpdate 하위 메서드
    void GroundUpdate()
    {
        isGround = FindGround(out var groundCP, contactPoints);
        thisAnimator.SetBool("isGround", isGround);

        if (isGround)
        {
            currentClimbDirection = Vector3.Cross(thisTransform.right, groundCP.normal);
            // slope
            isSteep = currentClimbDirection.y > Mathf.Cos(maxSlope * Mathf.Deg2Rad);

            var moveSpeed = thisAnimator.velocity.magnitude;
            if (!isSteep && moveSpeed > 0.1f)
            {
                if (currentClimbDirection.y < 0)
                {
                    // 위에서 아래로 누르기
                    thisRigidbody.velocity = new Vector3(0, Mathf.Tan(currentClimbDirection.y) * 1f * moveSpeed, 0);
                }
                else
                {
                    // 아래에서 위로 올리기
                    thisRigidbody.velocity = new Vector3(0, Mathf.Tan(currentClimbDirection.y) * 0.8f * moveSpeed, 0);
                }
            }
        }

        if (isGround && !isSteep)
        {
            thisAnimator.applyRootMotion = true;
        }
        else
        {
            thisAnimator.applyRootMotion = false;
        }

        contactPoints.Clear();

        lastVelocity = thisRigidbody.velocity;
    }
    #endregion

    #region Update 하위 메서드
    void MoveUpdate()
    {
        var moveVelocityRaw = isGround ? characterInput.MoveInput : Vector2.zero;
        moveVelocity = Vector2.SmoothDamp(moveVelocity, moveVelocityRaw, ref moveVelocitySmooth, moveVelocitySmoothTime);


        var moveMagnitude = moveVelocity.magnitude;
        var moveRawMagnitude = moveVelocityRaw.magnitude;
        if (moveRawMagnitude > 0.1f)
        {
            if (CanRotate)
            {
                LookAtByCamera(moveVelocityRaw);
            }
            thisAnimator.SetFloat("ySpeed", moveMagnitude);
            thisAnimator.SetBool("isMove", true);
        }
        else
        {
            //animator.SetFloat("ySpeed", 0);
            thisAnimator.SetBool("isMove", false);
        }

        // 방향 업데이트
        characterWorldAngle = Mathf.SmoothDampAngle(characterWorldAngle, characterWorldAngleTarget, ref characterWorldAngleSmooth, characterWorldAngleSmoothTime);

        //Debug.Log(characterWorldAngleSmoothTime * (1 - Mathf.Min(moveMagnitude, 1)));
        // 카메라가 바라보는 방향
        thisTransform.rotation = Quaternion.Euler(0, characterWorldAngle, 0);

        thisAnimator.SetBool("canRotate", CanRotate);
    }

    void AttackUpdate()
    {
        if (Animator.GetBool("doAttacking") && !BlockAttack)
        {
            animatorTriggerManager.CancelTrigger("doWeaponChange");
            //Animator.ResetTrigger("doWeaponChange");
        }

        if (BlockAttack)
        {
            animatorTriggerManager.CancelTrigger("doAttacking");
        }

        //thisAnimator.SetBool("doAttacking", DoAttacking);
        thisAnimator.SetBool("isAttacking", IsAttacking);
    }

    void EvadeUpdate()
    {
        if (!isEvadingChecked)
        {
            if (IsEvading)
            {
                isEvadingChecked = true;
                //noDamageTime = characterEquipment.WeaponData.NoDamageTimeByEvasion;
            }
        }
        else
        {
            if (!IsEvading)
            {
                //isEvadingChecked = false;
            }
        }

        Animator.SetBool("isEvade", IsEvade);
        Animator.SetBool("isImpact", IsImpact);
    }

    // 무기 변환 예약
    int reservedWeaponSwitchSlot;
    void WeaponSwitchUpdate()
    {
        var input = CharacterInput;
        var status = Status;

        int sortedWeaponSlotCount = status.SortedWeaponSlotCount;
        if (input.MaxWeaponSwitchInput != sortedWeaponSlotCount)
        {
            input.MaxWeaponSwitchInput = sortedWeaponSlotCount;
        }

        if (!Animator.GetBool("doWeaponChange") && switchingCooltime == 0)
        {
            input.LockWeaponSwitch = false;
            var currentWeaponSwitchInput = input.WeaponSwitchInput;
            if (reservedWeaponIndex != currentWeaponSwitchInput)
            {
                reservedWeaponIndex = currentWeaponSwitchInput;
                // 애니메이션 이벤트가 나와야 최종적으로 무기 교체가능.
                //Animator.SetTrigger("doWeaponChange");
                animatorTriggerManager.SetTrigger("doWeaponChange", doWeaponChangeTime, () =>
                {
                    // 트리거 취소로 인한 롤백
                    input.WeaponSwitchInput = reservedWeaponIndex = Status.CurrentWeaponSlotIndex;
                });
            }
        }
        else
        {
            input.LockWeaponSwitch = true;
        }
    }

    private void LockonUpdate()
    {
        var input = characterInput;

        if (input.LockonInput)
        {
        }

        if (isLockon)
        {
            // 대상 주시
        }
    }

    void AttackCancelUpdate()
    {
        if (!IsAttacking)
        {
            cancelAttack = false;
        }
    }

    void TimeUpdate()
    {
        var deltaTime = Time.deltaTime;
        Mathx.TimeToZero(ref noDamageTime, deltaTime);
        Mathx.TimeToZero(ref switchingCooltime, deltaTime);
    }
    #endregion

    /// <summary>
    /// 애니메이션 속도 갱신
    /// </summary>
    void UpdateAnimationSpeed()
    {
        Animator.SetFloat("attackSpeed", characterEquipment.WeaponData.AttackSpeed);
        //Animator.SetFloat("evadeSpeed", characterEquipment.WeaponData.eva);
        //Animator.SetFloat("moveSpeed", status.)
    }

    private void OnGUI()
    {
        GUILayout.TextArea($"weapon slot: {status.CurrentWeaponSlotIndex}");
        GUILayout.TextArea($"switch cooltime: {switchingCooltime}");
        GUILayout.TextArea($"IsAttacking: {IsAttacking}");
        GUILayout.TextArea($"doWeaponChange: {Animator.GetBool("doWeaponChange")}");
    }

    public void AttackInputHandle()
    {
        //attackInputTime = attackInputRate;
        DoAttack();
    }

    public void EvadeInputHandle()
    {
        //SendMessage("DamageTrigger_EndTrigger");
        animatorTriggerManager.SetTrigger("doEvading", doEvadeTime);
        animatorTriggerManager.SetTrigger("doBaseCancel", doEvadeTime);
        cancelAttack = true;
    }

    /// <summary>
    /// 애니메이터 이벤트에 의해 호출
    /// </summary>
    void WeaponChange()
    {
        status.CurrentWeaponSlotIndex = reservedWeaponIndex;

        var weapon = status.GetWeaponSlot(status.CurrentWeaponSlotIndex);
        if (characterEquipment.WeaponData != weapon)
        {
            characterEquipment.WeaponData = weapon;

            Debug.Log($"무기 '{characterEquipment.WeaponData.WeaponName}'로 변경");
            UpdateAnimationSpeed();
            // 쿨타임 적용
            switchingCooltime = Status.Data.SwitchingCoolTime;
        }
    }

    /// <summary>
    /// 애니메이터 이벤트
    /// </summary>
    void Behaviour_WeaponChange()
    {
        WeaponChange();
    }

    private void OnCollisionEnter(Collision collision)
    {
        contactPoints.AddRange(collision.contacts);
    }
    private void OnCollisionStay(Collision collision)
    {
        contactPoints.AddRange(collision.contacts);
    }

    bool FindGround(out ContactPoint groundCP, List<ContactPoint> allCPs)
    {
        groundCP = default(ContactPoint);

        if (contactPoints.Capacity == 0)
        {
            return true;
        }

        bool found = false;
        foreach (ContactPoint cp in allCPs)
        {
            //Pointing with some up direction
            if (cp.normal.y > 0.0001f && (found == false || cp.normal.y > groundCP.normal.y))
            {
                groundCP = cp;
                found = true;
            }
        }

        return found;
    }

    void SlopeUpdate()
    {

    }

    #region 쓰이지 못한 코드 조각
    //bool FindStep(out Vector3 stepUpOffset, List<ContactPoint> allCPs, ContactPoint groundCP)
    //{
    //    stepUpOffset = default;

    //    //No chance to step if the player is not moving
    //    var currVelocity = rigidbody.velocity;
    //    Vector2 velocityXZ = new Vector2(currVelocity.x, currVelocity.z);
    //    if (velocityXZ.sqrMagnitude < 0.0001f)
    //        return false;

    //    foreach (ContactPoint cp in allCPs)
    //    {
    //        bool test = ResolveStepUp(out stepUpOffset, cp, groundCP);
    //        if (test)
    //            return test;
    //    }
    //    return false;
    //}

    //bool ResolveStepUp(out Vector3 stepUpOffset, ContactPoint stepTestCP, ContactPoint groundCP)
    //{
    //    stepUpOffset = default;
    //    Collider stepCol = stepTestCP.otherCollider;

    //    //( 1 ) Check if the contact point normal matches that of a step (y close to 0)
    //    if (Mathf.Abs(stepTestCP.normal.y) >= 0.01f)
    //    {
    //        return false;
    //    }

    //    //( 2 ) Make sure the contact point is low enough to be a step
    //    if (!(stepTestCP.point.y - groundCP.point.y < maxStepHeight))
    //    {
    //        return false;
    //    }

    //    //( 3 ) Check to see if there's actually a place to step in front of us
    //    //Fires one Raycast
    //    RaycastHit hitInfo;
    //    float stepHeight = groundCP.point.y + maxStepHeight + 0.0001f;
    //    Vector3 stepTestInvDir = new Vector3(-stepTestCP.normal.x, 0, -stepTestCP.normal.z).normalized;
    //    Vector3 origin = new Vector3(stepTestCP.point.x, stepHeight, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
    //    Vector3 direction = Vector3.down;
    //    if (!(stepCol.Raycast(new Ray(origin, direction), out hitInfo, maxStepHeight)))
    //    {
    //        return false;
    //    }

    //    //We have enough info to calculate the points
    //    Vector3 stepUpPoint = new Vector3(stepTestCP.point.x, hitInfo.point.y + 0.0001f, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
    //    Vector3 stepUpPointOffset = stepUpPoint - new Vector3(stepTestCP.point.x, groundCP.point.y, stepTestCP.point.z);

    //    //We passed all the checks! Calculate and return the point!
    //    stepUpOffset = stepUpPointOffset;
    //    return true;
    //}
    #endregion

    void LookAtByCamera(Vector2 direction)
    {
        characterWorldAngleTarget = mainCameraTransform.rotation.eulerAngles.y + Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg - 90;
    }
}