using UnityEngine;

public class HitNotifier : MonoBehaviour
{
    public delegate void LandedHitDelegate(Collision2D collision);
    public event LandedHitDelegate LandedHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LandedHit?.Invoke(collision);
    }
}
