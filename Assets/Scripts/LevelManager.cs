using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerCameraPrefab;
    [SerializeField] private Transform m_PlayerSpawnPoint;

    [SerializeField] private Material m_FullscreenMaskMat;
    [SerializeField] private float m_StartEffectDuration = 1.5f;
    [SerializeField] private AnimationCurve m_LevelStartEffectAnimCurve;
    [SerializeField] private float m_EndEffectDuration = 3.0f;
    [SerializeField] private AnimationCurve m_LevelEndEffectAnimCurve;

    [SerializeField] private SOFloat scoreSO;

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

        scoreSO.Value = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var playerCameraGO = Instantiate(m_PlayerCameraPrefab, m_PlayerSpawnPoint.position, Quaternion.identity);
        var player = playerCameraGO.GetComponentInChildren<Player>();

        m_FullscreenMaskMat.SetFloat(k_ScreenRatioPropId, 1f / (Screen.height / (float)Screen.width));

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
        m_FullscreenMaskMat.SetFloat(k_UVScalePropId, initialScale);

        while (timer < m_EndEffectDuration)
        {
            yield return null;

            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / m_EndEffectDuration);
            float scale = Mathf.Lerp(initialScale, 500f, m_LevelEndEffectAnimCurve.Evaluate(t));
            m_FullscreenMaskMat.SetFloat(k_UVScalePropId, scale);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator PlayLevelStartAnim()
    {
        float initialScale = 500f;
        float timer = 0f;
        m_FullscreenMaskMat.SetFloat(k_UVScalePropId, initialScale);

        while (timer < m_StartEffectDuration)
        {
            yield return null;

            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / m_StartEffectDuration);
            float scale = Mathf.Lerp(initialScale, 0.01f, m_LevelStartEffectAnimCurve.Evaluate(t));
            m_FullscreenMaskMat.SetFloat(k_UVScalePropId, scale);
        }
    }

    #region Material Properties Ids
    private static readonly int k_ScreenRatioPropId = Shader.PropertyToID("_ScreenRatio");
    private static readonly int k_UVScalePropId = Shader.PropertyToID("_UVScale");
    #endregion
}
