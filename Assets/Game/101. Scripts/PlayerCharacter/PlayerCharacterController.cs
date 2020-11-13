using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector2 lookInput;
    [SerializeField] bool sprintInput;
    [SerializeField] UnityEvent attackInputReceived;

    public Vector2 MoveInput => moveInput;
    public Vector2 LookInput => lookInput;
    public bool SprintInput => sprintInput;
    public UnityEvent AttackInputReceived => attackInputReceived;

    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    public void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
    }

    public void OnSprint(InputValue input)
    {
        sprintInput = input.Get<bool>();
    }

    public void OnAttack(InputValue input)
    {
        if (attackInputReceived != null)
            attackInputReceived.Invoke();
    }
}