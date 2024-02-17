using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public Animator anim;

    private static EnemyHealth s_InstanceH;
    public static EnemyHealth Instance => s_InstanceH;

    [SerializeField] private SOFloat scoreSO;
    [SerializeField] private GameObject m_DeathVFX;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    void Die()
    {
        scoreSO.Value += 100;
        if (m_DeathVFX)
            Instantiate(m_DeathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetBool("IsHit", true);
            health = 0;
            Die();
        }
    }
}
