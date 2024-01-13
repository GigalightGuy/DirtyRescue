using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerCameraPrefab;
    [SerializeField] private Transform m_PlayerSpawnPoint;

    [SerializeField] private Material m_FullscreenMaskMat;
    [SerializeField] private AnimationCurve m_LevelStartEffectAnimCurve;
    [SerializeField] private AnimationCurve m_LevelEndEffectAnimCurve;

    private int m_RecyclingScore = 0;

    private void Awake()
    {
        if (!m_PlayerCameraPrefab)
        {
            Debug.LogError("Player prefab not assigned!");
            return;
        }

        if (!m_PlayerSpawnPoint)
        {
            Debug.LogError("Player spawn point not assigned!");
            return;
        }

        var playerCameraGO = Instantiate(m_PlayerCameraPrefab, m_PlayerSpawnPoint.position, Quaternion.identity);
        var player = playerCameraGO.GetComponentInChildren<Player>();

        m_FullscreenMaskMat.SetFloat("_ScreenRatio", 1f / (Screen.height / (float)Screen.width));

        player.PlayerDead += OnPlayerDead;

        StartCoroutine(PlayLevelStartAnim());
    }

    private void OnPlayerDead()
    {
        StartCoroutine(PlayLevelLostAnim());
    }

    private IEnumerator PlayLevelLostAnim()
    {
        float initialScale = 0.01f;
        float timer = 0f;
        m_FullscreenMaskMat.SetFloat("_UVScale", initialScale);

        while (timer < 1f)
        {
            yield return null;

            timer += Time.deltaTime;
            timer = Mathf.Clamp01(timer);
            float scale = Mathf.Lerp(initialScale, 500f, m_LevelEndEffectAnimCurve.Evaluate(timer));
            m_FullscreenMaskMat.SetFloat("_UVScale", scale);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator PlayLevelStartAnim()
    {
        float initialScale = 500f;
        float timer = 0f;
        m_FullscreenMaskMat.SetFloat("_UVScale", initialScale);

        while (timer < 1f)
        {
            yield return null;

            timer += Time.deltaTime;
            timer = Mathf.Clamp01(timer);
            float scale = Mathf.Lerp(initialScale, 0.01f, m_LevelStartEffectAnimCurve.Evaluate(timer));
            m_FullscreenMaskMat.SetFloat("_UVScale", scale);
        }
    }
}
