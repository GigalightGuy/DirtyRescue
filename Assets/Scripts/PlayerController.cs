using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActions m_InputActions;
    private Player m_Player;

    private void Awake()
    {
        m_InputActions = new InputActions();
        m_InputActions.Player.Enable();
        m_Player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        m_InputActions.Player.Move.started += OnMoveInput;
        m_InputActions.Player.Move.performed += OnMoveInput;
        m_InputActions.Player.Move.canceled += OnMoveInput;

        m_InputActions.Player.Jump.performed += OnJumpInput;
        m_InputActions.Player.Jump.canceled += OnJumpInput;

        m_InputActions.Player.Attack.performed += OnAttackInput;

        m_InputActions.Player.Defend.performed += OnDefendInput;
        m_InputActions.Player.Defend.canceled += OnDefendInput;
    }

    private void OnDisable()
    {
        m_InputActions.Player.Move.started -= OnMoveInput;
        m_InputActions.Player.Move.performed -= OnMoveInput;
        m_InputActions.Player.Move.canceled -= OnMoveInput;

        m_InputActions.Player.Jump.performed -= OnJumpInput;
        m_InputActions.Player.Jump.canceled -= OnJumpInput;

        m_InputActions.Player.Attack.performed -= OnAttackInput;

        m_InputActions.Player.Defend.performed -= OnDefendInput;
        m_InputActions.Player.Defend.canceled -= OnDefendInput;
    }

    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        float input = ctx.ReadValue<float>();
        m_Player.SetMovementInput(input);
    }

    private void OnJumpInput(InputAction.CallbackContext ctx)
    {
        float input = ctx.ReadValue<float>();
        if (input > 0.5f)
        {
            m_Player.StartJumping();
        }
        else
        {
            m_Player.StopJumping();
        }
    }

    private void OnAttackInput(InputAction.CallbackContext ctx)
    {
        m_Player.Attack();
    }

    private void OnDefendInput(InputAction.CallbackContext ctx)
    {
        float input = ctx.ReadValue<float>();
        if (input > 0.5f)
        {
            m_Player.StartDefending();
        }
        else
        {
            m_Player.StopDefending();
        }
    }

}
