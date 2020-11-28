using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance
    {
        get;
        private set;
    }

    [Header("디버그용")]
    [SerializeField] bool mouseLock = true;
    [SerializeField] bool mouseDebug;
    /// <summary>
    /// 마우스의 이동을 막습니다.
    /// </summary>
    public bool IsMouseLock
    {
        get => mouseLock && !mouseDebug;
        set
        {
            mouseLock = value;

            UpdateMouseLock(IsMouseLock);
        }
    }

    public bool IsMouseDebug
    {
        get => mouseDebug;
        set
        {
            mouseDebug = value;

            UpdateMouseLock(IsMouseLock);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    private void Start()
    {
        UpdateMouseLock(true);
    }
    private void Update()
    {
        if (Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            IsMouseDebug = !IsMouseDebug;
        }
    }

    void UpdateMouseLock(bool mouseLocked)
    {
        Cursor.lockState = mouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}