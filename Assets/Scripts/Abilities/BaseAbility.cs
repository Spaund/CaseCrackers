using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{
    protected abstract void Perform();
    protected abstract void Cancel();

    public void Input(InputAction.CallbackContext input)
    {
        if (input.performed) Perform();
        if (input.canceled) Cancel();
    }
}
