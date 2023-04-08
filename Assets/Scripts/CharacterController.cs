using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public CharacterActions[] action;
    
    // int character stores the current character being possessed by the player
    [NonSerialized] public int character;

    public void Move(InputAction.CallbackContext context)
    {
        action[character].Move(context);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        action[character].Jump(context);
    }

    public void Ability(InputAction.CallbackContext context)
    {
        action[character].Ability(context);
    }

    public void ChangeCharacter(InputAction.CallbackContext context)
    {
        // Changes character with input
        if (context.performed)
        {
            character += Convert.ToInt16(context.ReadValue<float>());
            // Clamps value so it stays within array bounds
            character = Mathf.Clamp(character, 0, action.Length - 1);
        }
    }
}
