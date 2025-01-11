using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerControl control;

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
        control.PerformMove(input);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Input");
        if (context.started) control.PerformJump();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            control.PerformAttack();
        }
    }

    public void Heal(InputAction.CallbackContext context)
    {
        if (context.started && control.playerSO.canHeal && control.ShowHPMP().Item1 < control.playerSO.maxHealth)
        {
            Manager.GameManager.CallHeal();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started && control.playerSO.canDash)
        {
            Manager.GameManager.CallDash();
        }
    }

    public void Option(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Manager.GameManager.CallOnOption();
        }
    }
}
