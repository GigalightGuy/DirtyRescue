using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public enum State
    {
        None = 0,

        Idle,
        Running,
        Jumping,
        Attacking,
        Falling,
        Damaged,
        Rooted,
        Stunned,
        Dead
    }

    private static readonly int k_IdleAnimStateId = Animator.StringToHash("Idle");
    private static readonly int k_RunAnimStateId = Animator.StringToHash("Run");
    private static readonly int k_JumpAnimStateId = Animator.StringToHash("Jump");
    private static readonly int k_FallAnimStateId = Animator.StringToHash("Fall");
    private static readonly int k_DamagedAnimStateId = Animator.StringToHash("Damaged");
    private static readonly int k_TurtleSwingAnimStateId = Animator.StringToHash("TurtleSwing");

    private static Player s_Instance;
    public static Player Instance => s_Instance;

    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_Acceleration = 5f;
    [SerializeField] private float m_Deceleration = 7f;
    [SerializeField] private float m_AccelerationExponent = 1f;
    [SerializeField] private float m_BrakeForce = 0.2f;

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

    [SerializeField] private float m_TurtleSwingImpactFrameDuration = 0.1f;

    [Space]
    [SerializeField] private LayerMask m_GroundedLayerMask;

    private Animator m_Animator;
    private SpriteRenderer m_Sprite;

    private Rigidbody2D m_RB;

    private bool m_IsGrounded = false;

    private float m_MoveInput = 0f;

    private bool m_JumpInput = false;

    private float m_JumpInitialVelocity;
    private float m_MinJumpTime;

    private Timer m_JumpTimer;
    private Timer m_CoyoteTimer;

    private Timer m_JumpBufferTimer;
    private bool m_JumpInQueue = false;

    private Timer m_RootedTimer;

    private Timer m_TurtleSwingImpactFrameTimer;

    private Timer m_DamageStateTimer;

    private bool m_StopAttacking;

    private State m_CurrentState = State.Idle;

    private int m_Health;

    public int Health => m_Health;

    public delegate void PlayerDeadDelegate();
    public event PlayerDeadDelegate PlayerDead;

    private void Awake()
    {
        if (s_Instance)
        {
            Debug.LogError("Can't have 2 players spawned at the same time");
            Destroy(gameObject);
            return;
        }
        s_Instance = this;

        m_Animator = GetComponent<Animator>();
        m_Sprite = GetComponent<SpriteRenderer>();

        m_RB = GetComponent<Rigidbody2D>();
        m_RB.gravityScale = m_GravityMultiplier;

        m_JumpTimer = new Timer();
        m_CoyoteTimer = new Timer();
        m_JumpBufferTimer = new Timer();
        m_RootedTimer = new Timer();
        m_TurtleSwingImpactFrameTimer = new Timer();
        m_DamageStateTimer = new Timer();

        // Calculate jump initial velocity from max jump height
        float g = Physics2D.gravity.y * m_GravityMultiplier;
        float t = Mathf.Sqrt(-2f * m_MaxJumpHeight / g);
        float v0 = m_MaxJumpHeight / t - 0.5f * g * t;
        m_JumpInitialVelocity = v0;

        m_MinJumpTime = (-v0 + Mathf.Sqrt(v0 * v0 - 4f * 0.5f * g * (-m_MinJumpHeight))) / (g);

        m_Health = m_MaxHealth;
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();

        if (m_JumpInQueue && m_JumpBufferTimer.HasEnded())
        {
            m_JumpInQueue = false;
        }

        if (m_IsGrounded && m_JumpInQueue && !PauseMenu._isPaused)
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
                else if (Mathf.Abs(m_MoveInput) > 0.01f)
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
                else if (Mathf.Abs(m_MoveInput) < 0.01f)
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
            case State.Attacking:
                if (m_StopAttacking)
                {
                    m_CurrentState = State.Idle;
                    m_Animator.Play(k_IdleAnimStateId);
                }
                break;
            case State.Falling:
                if (m_IsGrounded)
                {
                    Land();
                }
                break;
            case State.Damaged:
                if (m_DamageStateTimer.HasEnded())
                {
                    m_CurrentState = State.Falling;
                }
                break;
            case State.Rooted:
                if (m_RootedTimer.HasEnded())
                {
                    Land();
                }
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
                HandleMovement();
                break;
            case State.Running:
                HandleMovement();
                HandleFlipping();
                break;
            case State.Jumping:
                HandleMovement();
                HandleFlipping();
                break;
            case State.Attacking:
                if (m_TurtleSwingImpactFrameTimer.HasEnded())
                {
                    m_Animator.speed = 1f;
                }
                Brake();
                break;
            case State.Falling:
                HandleMovement();
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
            case State.Rooted:
                break;
            case State.Dead:
                break;
            case State.None:
                Debug.LogError("Invalided player state!");
                break;
        }
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
            m_DamageStateTimer.Start(0.5f);
            m_Animator.Play(k_DamagedAnimStateId);
        }
    }

    public void Root(float duration)
    {
        m_RootedTimer.Start(duration);
        m_CurrentState = State.Rooted;
    }

    public void SetMovementInput(float input)
    {
        m_MoveInput = input;
    }

    public void StartJumping()
    {
        if (!PauseMenu._isPaused)
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
    }

    public void StopJumping()
    {
        m_JumpInput = false;
    }

    public void Attack()
    {
        if (!PauseMenu._isPaused)
        {
            m_CurrentState = State.Attacking;
            m_StopAttacking = false;
            m_Animator.Play(k_TurtleSwingAnimStateId);
            AudioManager.instance.PunchSFX();
        }
    }

    public void OnAttackFinished()
    {
        m_StopAttacking = true;
    }

    public void ProcessHit(Rigidbody2D rb, Vector2 direction)
    {
        m_Animator.speed = 0;
        m_TurtleSwingImpactFrameTimer.Start(m_TurtleSwingImpactFrameDuration);
        rb.AddForce(50f * direction, ForceMode2D.Impulse);
    }

    private void HandleMovement()
    {
        float desiredVel = m_MoveInput * m_MoveSpeed;
        float velDiff = desiredVel - m_RB.velocity.x;
        float accel = (Mathf.Abs(desiredVel) > 0.01f) ? m_Acceleration : m_Deceleration;
        float force = Mathf.Pow(Mathf.Abs(velDiff) * accel, m_AccelerationExponent) * Mathf.Sign(velDiff);

        m_RB.AddForce(force * Vector2.right);

        if (Mathf.Abs(m_MoveInput) < 0.01f)
        {
            HandleBraking();
        }
    }

    private void Brake()
    {
        float desiredVel = 0f;
        float velDiff = desiredVel - m_RB.velocity.x;
        float accel = (Mathf.Abs(desiredVel) > 0.01f) ? m_Acceleration : m_Deceleration;
        float force = Mathf.Pow(Mathf.Abs(velDiff) * accel, m_AccelerationExponent) * Mathf.Sign(velDiff);

        m_RB.AddForce(force * Vector2.right);

        HandleBraking();
    }

    private void HandleBraking()
    {
        if (m_IsGrounded)
        {
            float vel = m_RB.velocity.x;
            float brakeAmount = Mathf.Min(Mathf.Abs(vel), m_BrakeForce);
            brakeAmount *= -Mathf.Sign(vel);
            m_RB.AddForce(brakeAmount * Vector2.right, ForceMode2D.Impulse);
        }
    }

    private void Jump()
    {
        m_JumpInput = true;
        m_CurrentState = State.Jumping;
        m_JumpInQueue = false;
        m_RB.velocity = new Vector2(m_RB.velocity.x, m_JumpInitialVelocity);

        m_Animator.Play(k_JumpAnimStateId);

        m_JumpTimer.Start(m_MinJumpTime);

        AudioManager.instance.PlayerJump();
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
        float velocity = m_RB.velocity.x;
        if (Mathf.Abs(velocity) > 0.01f)
        {
            bool isLeft = velocity < 0f;
            Vector2 scale = transform.localScale;
            bool xScaleFlipped = scale.x < 0f;
            if (isLeft != xScaleFlipped)
            {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }
    }

    private void UpdateGroundedState()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.down, 0.3f, m_GroundedLayerMask);
        m_IsGrounded = hit.collider && Vector2.Dot(hit.normal, Vector2.up) > 0.5f;
    }
}
