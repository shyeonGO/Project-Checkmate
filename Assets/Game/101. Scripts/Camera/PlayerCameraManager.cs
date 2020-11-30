using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookCamera;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform targetTransform;
    [Range(0, 1)]
    [SerializeField] float lockonCameraHeight = 0.35f;

    Transform freeLookCameraTransform;
    float xspeedBackup;
    float yspeedBackup;
    public Transform TargetTransform
    {
        get => this.targetTransform;
        set
        {
            this.targetTransform = value;

            freeLookCamera.LookAt = value;


            if (value != null)
            {
                xspeedBackup = freeLookCamera.m_XAxis.m_MaxSpeed;
                yspeedBackup = freeLookCamera.m_YAxis.m_MaxSpeed;
                freeLookCamera.m_XAxis.m_MaxSpeed = 0;
                freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            }
            else
            {
                freeLookCamera.m_XAxis.m_MaxSpeed = xspeedBackup;
                freeLookCamera.m_YAxis.m_MaxSpeed = yspeedBackup;
            }
        }
    }

    private void Awake()
    {
        xspeedBackup = freeLookCamera.m_XAxis.m_MaxSpeed;
        yspeedBackup = freeLookCamera.m_YAxis.m_MaxSpeed;
        freeLookCameraTransform = freeLookCamera.transform;
    }

    private void Start()
    {
        TargetTransform = TargetTransform;
    }

    private void LateUpdate()
    {
        if (freeLookCamera.LookAt == null)
        {
            targetTransform = null;
            freeLookCamera.LookAt = playerTransform;
        }
        else if (targetTransform != null)
        {
            var freeLookForward = freeLookCameraTransform.forward.ToXZ();
            var angle = Vector2.SignedAngle(Vector2.up, (targetTransform.position - playerTransform.position).ToXZ());
            angle -= Vector2.Angle(Vector2.down, freeLookForward);

            freeLookCamera.m_XAxis.Value = -angle;
            freeLookCamera.m_YAxis.Value = lockonCameraHeight;
        }
    }
}