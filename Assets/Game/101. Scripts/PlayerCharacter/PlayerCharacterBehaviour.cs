using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterBehaviour : MonoBehaviour
{
    #region 인스펙터 변수
    [Header("Ground")]
    //[SerializeField] LayerMask layerMask;
    //[SerializeField] float maxStepHeight = 0.5f;
    //[SerializeField] float stepSearchOvershoot = 0.01f;
    [SerializeField] float maxSlope = 45;
    [Header("SmoothTime")]
    [SerializeField] float characterWorldAngleSmoothTime = 0.1f;
    [SerializeField] float moveVelocitySmoothTime = 0.1f;
    [Header("Debug")]
    // 착지 상태
    [SerializeField] bool isGround = false;
    // 가파른 상태
    [SerializeField] bool isSteep = false;
    //[SerializeField] int reservedAttackStep = 0;
    //[SerializeField] int currentAttackStep = 0;
    #endregion


    Transform thisTransform;
    PlayerCharacterController characterControl;
    Animator animator;
    Rigidbody rigidbody;

    Transform mainCameraTransform;

    float characterWorldAngle = 0;
    float characterWorldAngleTarget = 0;
    float characterWorldAngleSmooth = 0;
    Vector2 moveVelocity;
    Vector2 moveVelocitySmooth;

    Vector3 currentClimbDirection;

    float attackInputTime = 0;

    List<ContactPoint> contactPoints = new List<ContactPoint>(0);

    public bool DoAttacking => attackInputTime > 0;
    public bool IsAttacking => animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsTag("Attack");

    void Awake()
    {
        thisTransform = transform;
        characterControl = GetComponent<PlayerCharacterController>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

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
    }

    Vector3 lastVelocity;

    #region FixedUpdate 하위 메서드
    void GroundUpdate()
    {
        isGround = FindGround(out var groundCP, contactPoints);
        animator.SetBool("isGround", isGround);

        if (isGround)
        {
            currentClimbDirection = Vector3.Cross(thisTransform.right, groundCP.normal);
            //Debug.Log(currentClimbDirection);

            //animator.applyRootMotion = true;

            // slope
            isSteep = currentClimbDirection.y > Mathf.Cos(maxSlope * Mathf.Deg2Rad);

            var moveSpeed = animator.velocity.magnitude;
            //Debug.Log($"{moveSpeed:n5}");

            if (!isSteep && moveSpeed > 0.1f)
            {
                if (currentClimbDirection.y < 0)
                {
                    // 위에서 아래로 누르기
                    rigidbody.velocity = new Vector3(0, Mathf.Tan(currentClimbDirection.y) * 1f * moveSpeed, 0);
                }
                else
                {
                    // 아래에서 위로 올리기
                    rigidbody.velocity = new Vector3(0, Mathf.Tan(currentClimbDirection.y) * 0.8f * moveSpeed, 0);
                }
            }
            //if (FindStep(out var stepUpOffset, contactPoints, groundCP))
            //{
            //    rigidbody.position += stepUpOffset;
            //    rigidbody.velocity = lastVelocity;
            //}

            //var velocity = rigidbody.velocity;
            //if (moveVelocity.sqrMagnitude > 0.0001f)
            //{
            //    var forwardPosition = rigidbody.position + thisTransform.forward * 0.5f;
            //    var stepHeightVector = Vector3.up * maxStepHeight;
            //    if (Physics.Raycast(forwardPosition + stepHeightVector, Vector3.down, out var hit, maxStepHeight * 2))
            //    {
            //        velocity.y = (hit.point - rigidbody.position).y;
            //        velocity.y *= 4.5f;
            //        rigidbody.velocity = velocity;
            //        Debug.Log($"{velocity.y:n5}");
            //    }
            //}
        }

        if (isGround && !isSteep)
        {
            animator.applyRootMotion = true;
        }
        else
        {
            animator.applyRootMotion = false;
        }

        contactPoints.Clear();

        lastVelocity = rigidbody.velocity;
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
            animator.SetFloat("ySpeed", moveMagnitude);
            animator.SetBool("isMove", true);
        }
        else
        {
            //animator.SetFloat("ySpeed", 0);
            animator.SetBool("isMove", false);
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

        animator.SetBool("doAttacking", DoAttacking);
        //animator.SetInteger("reservedAttackStep", reservedAttackStep);
    }
    #endregion

    public void AttackInputHandle()
    {
        attackInputTime = 1;

        //var reservedAttackStep = animator.GetInteger("reservedAttackStep");
        //var currentAttackStep = animator.GetInteger("currentAttackStep");
        //if (reservedAttackStep <= currentAttackStep)
        //{
        //    reservedAttackStep += 1;
        //    animator.SetInteger("reservedAttackStep", reservedAttackStep);
        //}
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

    void LookAtByCamera(Vector2 direction)
    {
        characterWorldAngleTarget = mainCameraTransform.rotation.eulerAngles.y + Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg - 90;
    }
}