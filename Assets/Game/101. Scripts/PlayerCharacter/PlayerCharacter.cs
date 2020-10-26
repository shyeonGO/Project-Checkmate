using UnityEngine;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(PlayerCharacterControl))]
[RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookCamera;

    Transform thisTransform;
    PlayerCharacterControl characterControl;
    Animator animator;

    Vector3 moveVelocity;

    #region SmoothDamp
    #endregion

    public void Awake()
    {
        thisTransform = transform;
        characterControl = GetComponent<PlayerCharacterControl>();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        moveVelocity = characterControl.MoveInput;

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
        thisTransform.rotation = Quaternion.Euler(0, freeLookCamera.m_XAxis.Value + angle, 0);
    }
}