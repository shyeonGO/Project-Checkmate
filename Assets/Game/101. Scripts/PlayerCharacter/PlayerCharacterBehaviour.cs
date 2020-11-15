using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(PlayerCharacterEquipment))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterBehaviour : MonoBehaviour
{
    #region 인스펙터 변수
    [SerializeField]
    PlayerCharacterStatus status;
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
    #endregion


    Transform thisTransform;
    PlayerCharacterController characterControl;
    PlayerCharacterEquipment characterEquipment;
    Animator thisAnimator;
    Rigidbody thisRigidbody;

    Transform mainCameraTransform;

    float characterWorldAngle = 0;
    float characterWorldAngleTarget = 0;
    float characterWorldAngleSmooth = 0;
    Vector2 moveVelocity;
    Vector2 moveVelocitySmooth;

    Vector3 currentClimbDirection;

    float attackInputTime = 0;

    List<ContactPoint> contactPoints = new List<ContactPoint>(0);

    public Transform Transform => thisTransform;
    public Rigidbody Rigidbody => thisRigidbody;
    public Animator Animator => thisAnimator;
    public PlayerCharacterController CharacterController => characterControl;
    public PlayerCharacterStatus Status => status;
    public bool DoAttacking => attackInputTime > 0;
    public bool IsAttacking => DoAttacking || thisAnimator.GetCurrentAnimatorStateInfo(thisAnimator.GetLayerIndex("Base Layer")).IsTag("Attack");

    void Awake()
    {
        thisTransform = transform;
        characterControl = GetComponent<PlayerCharacterController>();
        characterEquipment = GetComponent<PlayerCharacterEquipment>();
        thisAnimator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody>();

        if (status == null)
            status = this.GetComponentOrNew<PlayerCharacterStatus>();

        mainCameraTransform = Camera.main.transform;

        var forward = thisTransform.forward;

        characterWorldAngle = characterWorldAngleTarget = thisTransform.rotation.eulerAngles.y;
    }

    private void Start()
    {
        characterControl.AttackInputReceived.AddListener(this.AttackInputHandle);
    }

    void FixedUpdate()
    {
        GroundUpdate();
    }

    private void Update()
    {
        MoveUpdate();
        AttackUpdate();
        WeaponSwitchUpdate();
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
        var moveVelocityRaw = isGround ? characterControl.MoveInput : Vector2.zero;
        moveVelocity = Vector2.SmoothDamp(moveVelocity, moveVelocityRaw, ref moveVelocitySmooth, moveVelocitySmoothTime);


        var moveMagnitude = moveVelocity.magnitude;
        var moveRawMagnitude = moveVelocityRaw.magnitude;
        if (moveRawMagnitude > 0.1f)
        {
            if (!IsAttacking)
                LookAtByCamera(moveVelocityRaw);
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
    }

    void AttackUpdate()
    {
        if (attackInputTime > 0)
        {
            attackInputTime -= Time.deltaTime;
        }
        else
        {
            attackInputTime = 0;
        }

        thisAnimator.SetBool("doAttacking", DoAttacking);
    }

    void WeaponSwitchUpdate()
    {
        // TODO: 공격도중에 무기를 바꾸면 공격이 캔슬되거나 공격이 끝날 때까지 대기하도록 해야함.
        var controller = CharacterController;
        var status = Status;

        int sortedWeaponSlotCount = status.SortedWeaponSlotCount;
        if (controller.MaxWeaponSwitchInput != sortedWeaponSlotCount)
        {
            controller.MaxWeaponSwitchInput = sortedWeaponSlotCount;
        }

        var currentWeaponIndex = controller.WeaponSwitchInput;
        if (status.CurrentWeaponSlotIndex != controller.WeaponSwitchInput)
        {
            status.CurrentWeaponSlotIndex = currentWeaponIndex;

            characterEquipment.WeaponData = status.GetWeaponSlot(currentWeaponIndex);

            Debug.Log($"무기 '{characterEquipment.WeaponData.WeaponName}'로 변경");
        }
    }
    #endregion

    public void AttackInputHandle()
    {
        attackInputTime = 1;
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