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

    public Vector2 MoveInput => moveInput;
    public Vector2 LookInput => lookInput;
    public bool SprintInput => sprintInput;
    public int WeaponSwitchInput => weaponSwitchInput;
    public UnityEvent AttackInputReceived => attackInputReceived;

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

    public void OnWeaponSwitch1(InputValue input)
    {
        if (maxWeaponSwitchInput >= 1)
        {
            weaponSwitchInput = 1;
        }
    }

    public void OnWeaponSwitch2(InputValue input)
    {
        if (maxWeaponSwitchInput >= 2)
        {
            weaponSwitchInput = 2;
        }
    }

    public void OnWeaponSwitch3(InputValue input)
    {
        if (maxWeaponSwitchInput >= 3)
        {
            weaponSwitchInput = 3;
        }
    }

    public void OnWeaponSwitch4(InputValue input)
    {
        if (maxWeaponSwitchInput >= 4)
        {
            weaponSwitchInput = 4;
        }
    }

    public void OnWeaponSwitchCycle(InputValue input)
    {
        weaponSwitchInput = (weaponSwitchInput % maxWeaponSwitchInput) + 1;
    }
}