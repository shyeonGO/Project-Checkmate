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
    [SerializeField] int maxWeaponSwitchInput = 4;
    [Header("디버깅")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector2 lookInput;
    [SerializeField] bool sprintInput;
    [SerializeField] int weaponSwitchInput = 1;
    [SerializeField] UnityEvent attackInputReceived;
    [SerializeField] UnityEvent evadeInputReceived;
    public Vector2 MoveInput => moveInput;
    public Vector2 LookInput => lookInput;
    public bool SprintInput => sprintInput;
    public int WeaponSwitchInput => weaponSwitchInput;
    public UnityEvent AttackInputReceived => attackInputReceived;
    public UnityEvent EvadeInputReceived => evadeInputReceived;

    public bool LockWeaponSwitch
    {
        get; set;
    }

    public int MaxWeaponSwitchInput
    {
        get => this.maxWeaponSwitchInput;
        set
        {
            this.maxWeaponSwitchInput = value;

            weaponSwitchInput = ((weaponSwitchInput - 1) % maxWeaponSwitchInput) + 1;
        }
    }

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
        sprintInput = input.isPressed;
    }

    public void OnAttack(InputValue input)
    {
        if (attackInputReceived != null)
            attackInputReceived.Invoke();
    }

    public void OnEvade(InputValue input)
    {
        if (evadeInputReceived != null)
            evadeInputReceived.Invoke();
    }

    public void OnWeaponSwitch1(InputValue input)
    {
        if (!LockWeaponSwitch && maxWeaponSwitchInput >= 1)
        {
            weaponSwitchInput = 1;
        }
    }

    public void OnWeaponSwitch2(InputValue input)
    {
        if (!LockWeaponSwitch && maxWeaponSwitchInput >= 2)
        {
            weaponSwitchInput = 2;
        }
    }

    public void OnWeaponSwitch3(InputValue input)
    {
        if (!LockWeaponSwitch && maxWeaponSwitchInput >= 3)
        {
            weaponSwitchInput = 3;
        }
    }

    public void OnWeaponSwitch4(InputValue input)
    {
        if (!LockWeaponSwitch&&maxWeaponSwitchInput >= 4)
        {
            weaponSwitchInput = 4;
        }
    }

    public void OnWeaponSwitchCycle(InputValue input)
    {
        if (!LockWeaponSwitch)
        weaponSwitchInput = (weaponSwitchInput % maxWeaponSwitchInput) + 1;
    }
}