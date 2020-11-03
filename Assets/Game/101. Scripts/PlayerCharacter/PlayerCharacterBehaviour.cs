using UnityEngine;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerCharacterBehaviour : MonoBehaviour
{
    #region 인스펙터 변수
    // 코드에서 이 변수에 직접 접근하는 것은 지양하세요!
    [SerializeField] float characterWorldAngleSmoothTime = 0.1f;
    [SerializeField] float moveVelocitySmoothTime = 0.1f;
    #endregion


    Transform thisTransform;
    PlayerCharacterController characterControl;
    Animator animator;

    Transform mainCameraTransform;

    float characterWorldAngle = 0;
    float characterWorldAngleTarget = 0;
    float characterWorldAngleSmooth = 0;
    Vector2 moveVelocity;
    Vector2 moveVelocitySmooth;

    public void Awake()
    {
        thisTransform = transform;
        characterControl = GetComponent<PlayerCharacterController>();
        animator = GetComponent<Animator>();

        mainCameraTransform = Camera.main.transform;

        var forward = thisTransform.forward;

        characterWorldAngle = characterWorldAngleTarget = Mathf.Atan2(forward.y, -forward.x) * Mathf.Rad2Deg - 0;
    }

    public void FixedUpdate()
    {
        var moveVelocityRaw = characterControl.MoveInput;
        moveVelocity = Vector2.SmoothDamp(moveVelocity, moveVelocityRaw, ref moveVelocitySmooth, moveVelocitySmoothTime);

        var moveMagnitude = moveVelocity.magnitude;
        var moveRawMagnitude = moveVelocityRaw.magnitude;
        if (moveRawMagnitude > 0.1f)
        {
            LookAt(moveVelocityRaw);
            animator.SetFloat("ySpeed", moveMagnitude);
            animator.SetBool("isMove", true);
        }
        else
        {
            //animator.SetFloat("ySpeed", 0);
            animator.SetBool("isMove", false);
        }

        // 방향 업데이트
        characterWorldAngle = Mathf.SmoothDampAngle(characterWorldAngle, characterWorldAngleTarget, ref characterWorldAngleSmooth, characterWorldAngleSmoothTime * (1.25f - Mathf.Min(moveMagnitude, 1)));

        Debug.Log(characterWorldAngleSmoothTime * (1 - Mathf.Min(moveMagnitude, 1)));
        // 카메라가 바라보는 방향
        thisTransform.rotation = Quaternion.Euler(0, characterWorldAngle, 0);
    }

    void LookAt(Vector2 direction)
    {
        characterWorldAngleTarget = mainCameraTransform.rotation.eulerAngles.y + Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg - 90;
    }
}