using UnityEngine;

public class HitHandler : MonoBehaviour
{
    private Player m_Player;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        var direction = other.transform.position - transform.position;
        m_Player.ProcessHit(rb, direction);
    }
}
