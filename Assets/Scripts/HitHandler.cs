using UnityEngine;

public class HitHandler : MonoBehaviour
{
    private Player m_Player;

    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemyHealth = other.GetComponentInParent<EnemyHealth>();
        var rb = other.attachedRigidbody;
        var direction = m_Player.transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        m_Player.ProcessHit(enemyHealth, rb, direction);

        AudioManager.instance.PunchSFX();
    }
}
