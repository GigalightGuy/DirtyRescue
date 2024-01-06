using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        None = 0,

        Idle,
        Running,
        Jumping,
        Falling,
        Stunned
    }

    [SerializeField] private float m_Movespeed = 5f;
    [SerializeField] private float m_RampUpTime = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] private float m_MinJumpHeight = 1f;
    [SerializeField] private float m_MaxJumpHeight = 2f;
    [SerializeField] private float m_GravityMultiplier = 2f;
    [SerializeField] private float m_GravityMultiplierWhenFalling = 4f;
    [SerializeField] private float m_JumpBufferingTime = 0.3f;
    [SerializeField] private float m_CoyoteTime = 0.2f;

    [SerializeField] private float m_MaxFallSpeed = 15f;

    [Space]
    [SerializeField] private LayerMask m_GroundedLayerMask;

    private Rigidbody2D m_RB;

    private bool m_IsGrounded = false;

    private float m_MovementInput = 0f;
    private float m_Movement = 0f;

    private bool m_IsJumping = false;

    private float m_JumpInitialVelocity;
    private float m_MinJumpTime;

    private Timer m_JumpTimer;
    private Timer m_CoyoteTimer;

    private Timer m_JumpBufferTimer;
    private bool m_JumpInQueue = false;

    private State m_CurrentState = State.Idle;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_RB.gravityScale = m_GravityMultiplier;

        m_JumpTimer = new Timer();
        m_CoyoteTimer = new Timer();
        m_JumpBufferTimer = new Timer();

        // Calculate jump initial velocity from max jump height
        float g = Physics2D.gravity.y * m_GravityMultiplier;
        float t = Mathf.Sqrt(-2f * m_MaxJumpHeight / g);
        float v0 = m_MaxJumpHeight / t - 0.5f * g * t;
        m_JumpInitialVelocity = v0;

        m_MinJumpTime = (-v0 + Mathf.Sqrt(v0 * v0 - 4f * 0.5f * g * (-m_MinJumpHeight))) / (g);
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();

        switch (m_CurrentState)
        {
            case State.Idle:
                break;
            case State.Running:
                break;
            case State.Jumping:
                break;
            case State.Falling:
                break;
            case State.Stunned:
                break;
            case State.None:
                Debug.LogError("Invalided player state!");
                break;
        }

        if (m_JumpInQueue && m_JumpBufferTimer.HasEnded())
        {
            m_JumpInQueue = false;
        }

        if (m_IsGrounded && m_JumpInQueue)
        {
            Jump();
        }

        if (!m_IsGrounded && !m_IsJumping && m_JumpTimer.HasEnded())
        {
            m_RB.gravityScale = m_GravityMultiplierWhenFalling;
        }

        if (!m_IsGrounded && m_IsJumping && m_RB.velocity.y <= 0f)
        {
            m_IsJumping = false;
            m_RB.gravityScale = m_GravityMultiplierWhenFalling;
        }

        if (m_RB.velocity.y < -m_MaxFallSpeed)
        {
            m_RB.velocity = new Vector2(m_RB.velocity.x, -m_MaxFallSpeed);
        }

        Mathf.Sign(m_MovementInput);
        if (Mathf.Abs(m_Movement - m_MovementInput) > 0.01f)
        {
            float diff = m_MovementInput - m_Movement;
            m_Movement += Mathf.Min(diff, Mathf.Sign(diff) * (1f / m_RampUpTime) * Time.fixedDeltaTime);
        }
        else
        {
            m_Movement = m_MovementInput;
        }

        m_RB.velocity = new Vector2(m_Movespeed * m_Movement, m_RB.velocity.y);
    }

    public void SetMovementInput(float input)
    {
        m_MovementInput = input;
    }

    public void StartJumping()
    {
        if (m_IsGrounded || !m_CoyoteTimer.HasEnded())
        {
            Jump();
        }
        else
        {
            m_JumpBufferTimer.Start(m_JumpBufferingTime);
            m_JumpInQueue = true;
        }
    }

    private void Jump()
    {
        m_IsJumping = true;
        m_JumpInQueue = false;
        m_RB.velocity = new Vector2(m_RB.velocity.x, m_JumpInitialVelocity);

        m_JumpTimer.Start(m_MinJumpTime);
    }

    public void StopJumping()
    {
        m_IsJumping = false;
    }

    private void UpdateGroundedState()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.down, 0.3f, m_GroundedLayerMask);
        bool grounded = hit.collider && Vector2.Dot(hit.normal, Vector2.up) > 0.5f;

        if (grounded && !m_IsGrounded)
        {
            m_IsGrounded = true;
            m_RB.gravityScale = m_GravityMultiplier;

            if (m_IsJumping)
            {
                m_IsJumping = false;
            }
        }

        if (!grounded && m_IsGrounded)
        {
            m_IsGrounded = false;

            if (!m_IsJumping)
            {
                m_CoyoteTimer.Start(m_CoyoteTime);
            }
        }
    }
}
