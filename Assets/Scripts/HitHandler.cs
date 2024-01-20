using UnityEngine;

public class HitHandler : MonoBehaviour
{
    private Player m_Player;
    private HitNotifier[] m_Notifiers;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_Notifiers = GetComponentsInChildren<HitNotifier>();

        foreach (var notifier in m_Notifiers)
        {
            notifier.LandedHit += OnLandedHit;
        }
    }

    private void OnLandedHit(Collision2D collision)
    {
        m_Player.ProcessHit();
    }
}
