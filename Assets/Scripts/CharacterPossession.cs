using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPossession : MonoBehaviour
{
    private CharacterMovement[] movement;
    private Ability[] ability;
    private int _current;

    private void OnEnable()
    {
        // Gets components on the player (must be children of the Object this Component is on)
        movement = GetComponentsInChildren<CharacterMovement>();
        ability = GetComponentsInChildren<Ability>();
        
        /* Debug */
        // Throws an error in console if there's not the same amount of movement and ability components
        // This prevents _current from getting the arrays out of bounds
        if (movement.Length != ability.Length)
            Debug.LogError("Movement.Length does not equal Ability.Length. Please check if some player component is missing. Movement.length = " +
                           movement.Length + "; Ability.Length = " + ability.Length + ".");
        // Throes an error in console if the number of children is not the same as the number of characters
        // This prevents a character from not being a children of this GameObject
        if (movement.Length != GameObject.FindGameObjectsWithTag("Character").Length)
            Debug.LogError("Children number does not equal Player number. Verify tags are setup properly and that all Characters are parent of " +
                           transform.name + ".");
    }
    
    public void ChangeCharacter(InputAction.CallbackContext input)
    {
        _current += Convert.ToInt16(input.ReadValue<float>());
        // Clamps _current value so movement[] and ability[] don't go out of bounds
        _current = Mathf.Clamp(_current, 0, movement.Length - 1);
    }

    // This passes the inputs to the current character
    // There's nothing to see on this functions
    public void Move(InputAction.CallbackContext input)
    {
        movement[_current].Move(input);
    }
    public void Look(InputAction.CallbackContext input)
    {
        movement[_current].Mouse(input);
    }
    public void Ability(InputAction.CallbackContext input)
    {
        ability[_current].Input(input);
    }
}
