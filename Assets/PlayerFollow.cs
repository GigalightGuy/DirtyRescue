using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private Player m_Player;

    private void Update()
    {
        transform.position = new(m_Player.transform.position.x, transform.position.y, m_Player.transform.position.z);

        if (m_Player.IsGrounded)
        {
            transform.position = new(transform.position.x, m_Player.transform.position.y, transform.position.z);
        }
    }
}
