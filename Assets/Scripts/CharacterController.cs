using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public CharacterActions action;

    public void Move(InputAction.CallbackContext context)
    {
        action.Move(context);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        action.Jump(context);
    }

    public void Ability(InputAction.CallbackContext context)
    {
        action.Ability(context);
    }

}
