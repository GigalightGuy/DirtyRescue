using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public enum State
    {
        None = 0,

        Idle,
        Running,
        Jumping,
        Falling,
        Damaged,
        Stunned,
        Dead
    }

    private static readonly int k_IdleAnimStateId = Animator.StringToHash("Idle");
    private static readonly int k_RunAnimStateId = Animator.StringToHash("Run");
    private static readonly int k_JumpAnimStateId = Animator.StringToHash("Jump");
    private static readonly int k_FallAnimStateId = Animator.StringToHash("Fall");
    private static readonly int k_DamagedAnimStateId = Animator.StringToHash("Damaged");

    [Header("Movement")]
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

    [Header("Combat stats")]
    [SerializeField] private int m_MaxHealth = 6;
    [SerializeField] private int m_Damage = 1;

    [Space]
    [SerializeField] private LayerMask m_GroundedLayerMask;

    private Animator m_Animator;
    private SpriteRenderer m_Sprite;

    private Rigidbody2D m_RB;

    private bool m_IsGrounded = false;

    private float m_MovementInput = 0f;
    private float m_Movement = 0f;

    private bool m_JumpInput = false;

    private float m_JumpInitialVelocity;
    private float m_MinJumpTime;

    private Timer m_JumpTimer;
    private Timer m_CoyoteTimer;

    private Timer m_JumpBufferTimer;
    private bool m_JumpInQueue = false;

    private bool m_IsFlipped;

    private State m_CurrentState = State.Idle;

    private int m_Health;

    public int Health => m_Health;

    public delegate void PlayerDeadDelegate();
    public event PlayerDeadDelegate PlayerDead;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Sprite = GetComponent<SpriteRenderer>();

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

        m_Health = m_MaxHealth;
    }

    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();

        if (m_JumpInQueue && m_JumpBufferTimer.HasEnded())
        {
            m_JumpInQueue = false;
        }

        if (m_IsGrounded && m_JumpInQueue)
        {
            Jump();
        }

        switch (m_CurrentState)
        {
            case State.Idle:
                if (!m_IsGrounded)
                {
                    m_CoyoteTimer.Start(m_CoyoteTime);
                    StartFalling();
                }
                else if (Mathf.Abs(m_Movement) > 0.01f)
                {
                    m_CurrentState = State.Running;
                    m_Animator.Play(k_RunAnimStateId);
                }
                break;
            case State.Running:
                if (!m_IsGrounded)
                {
                    m_CoyoteTimer.Start(m_CoyoteTime);
                    StartFalling();
                }
                else if (Mathf.Abs(m_Movement) < 0.01f)
                {
                    m_CurrentState = State.Idle;
                    m_Animator.Play(k_IdleAnimStateId);
                }
                break;
            case State.Jumping:
                if (m_JumpTimer.HasEnded() && m_IsGrounded)
                {
                    Land();
                }
                else if (m_RB.velocity.y <= 0f)
                {
                    StartFalling();
                }
                else if (!m_JumpInput && m_JumpTimer.HasEnded())
                {
                    m_RB.gravityScale = m_GravityMultiplierWhenFalling;
                }
                break;
            case State.Falling:
                if (m_IsGrounded)
                {
                    Land();
                }
                break;
            case State.Damaged:
                m_CurrentState = State.Falling;
                break;
            case State.Stunned:
                break;
            case State.Dead:
                break;
            case State.None:
                Debug.LogError("Invalided player state!");
                break;
        }

        switch (m_CurrentState)
        {
            case State.Idle:
                break;
            case State.Running:
                HandleFlipping();
                break;
            case State.Jumping:
                HandleFlipping();
                break;
            case State.Falling:
                HandleFlipping();
                if (m_RB.velocity.y < -m_MaxFallSpeed)
                {
                    m_RB.velocity = new Vector2(m_RB.velocity.x, -m_MaxFallSpeed);
                }
                break;
            case State.Damaged:

                break;
            case State.Stunned:
                break;
            case State.Dead:
                break;
            case State.None:
                Debug.LogError("Invalided player state!");
                break;
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

    public void TakeDamage(int damage)
    {
        if (m_CurrentState == State.Dead) return;

        m_Health -= damage;
        m_RB.velocity = Vector2.zero;

        if (m_Health <= 0)
        {
            m_CurrentState = State.Dead;
            PlayerDead?.Invoke();
            m_Health = 0;
        }
        else
        {
            m_CurrentState = State.Damaged;
            m_Animator.Play(k_DamagedAnimStateId);
        }
    }

    public void SetMovementInput(float input)
    {
        m_MovementInput = input;
    }

    public void StartJumping()
    {
        if (m_CurrentState != State.Jumping && m_IsGrounded || !m_CoyoteTimer.HasEnded())
        {
            Jump();
        }
        else
        {
            m_JumpBufferTimer.Start(m_JumpBufferingTime);
            m_JumpInQueue = true;
        }
    }

    public void StopJumping()
    {
        m_JumpInput = false;
    }

    private void Jump()
    {
        m_JumpInput = true;
        m_CurrentState = State.Jumping;
        m_JumpInQueue = false;
        m_RB.velocity = new Vector2(m_RB.velocity.x, m_JumpInitialVelocity);

        m_Animator.Play(k_JumpAnimStateId);

        m_JumpTimer.Start(m_MinJumpTime);
    }

    private void Land()
    {
        m_RB.gravityScale = m_GravityMultiplier;
        m_CurrentState = State.Idle;
        m_Animator.Play(k_IdleAnimStateId);
    }

    private void StartFalling()
    {
        m_RB.gravityScale = m_GravityMultiplierWhenFalling;
        m_CurrentState = State.Falling;
        m_Animator.Play(k_FallAnimStateId);
    }

    private void HandleFlipping()
    {
        if (Mathf.Abs(m_Movement) > 0.01f)
        {
            bool isLeft = Mathf.Sign(m_Movement) < 0f;
            if (isLeft != m_IsFlipped)
            {
                m_IsFlipped = isLeft;
                m_Sprite.flipX = m_IsFlipped;
            }
        }
    }

    private void UpdateGroundedState()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.down, 0.3f, m_GroundedLayerMask);
        m_IsGrounded = hit.collider && Vector2.Dot(hit.normal, Vector2.up) > 0.5f;
    }
}
