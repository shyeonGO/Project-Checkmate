using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacterControl : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintInput { get; private set; }

    public void OnMove(InputValue input)
    {
        MoveInput = input.Get<Vector2>();
    }

    public void OnLook(InputValue input)
    {
        LookInput = input.Get<Vector2>();
    }

    public void OnSprint(InputValue input)
    {
        SprintInput = input.Get<bool>();
    }
}
