using UnityEngine;

public class Timer
{
    public void Start(float duration) => m_End = Time.time + duration;
    public bool HasEnded() => m_End <= Time.time;

    private float m_End;
}
