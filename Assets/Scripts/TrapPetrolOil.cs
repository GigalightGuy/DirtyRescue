using UnityEngine;

public class TrapPetrolOil : MonoBehaviour
{
    [SerializeField] private bool onFire;
    [SerializeField] private float timer;
    [SerializeField] private bool isIn = false;

    [SerializeField] private GameObject m_FireVFX;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        if (onFire)
        {
            SetOnFire();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire == true)
        {
            if (isIn)
            {
                timer += Time.deltaTime;
                if (timer > 2f)
                {
                    Player.Instance.TakeDamage(1);
                    timer = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onFire == true && collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(1);
            isIn = true;           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isIn = false;
        timer = 0;
    }

    public void SetOnFire()
    {
        onFire = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        m_FireVFX.SetActive(true);
    }
}
