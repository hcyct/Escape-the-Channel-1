using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [Header("操作提示面板")]
    [SerializeField] private GameObject controlHintPanel;

    [Header("渐变设置")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("设置面板")]
    [SerializeField] private GameObject settingsPanel;

    [Header("音乐控制")]
    public AudioSource musicSource;
    public Image musicToggleButtonImage;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    private bool isPaused = false;
    private bool isMusicMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Update()
    {
        HandleControlHint();
    }

    private void HandleControlHint()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (controlHintPanel != null)
            {
                controlHintPanel.SetActive(true);
            }

            Time.timeScale = 0f; // 暂停游戏
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (controlHintPanel != null)
            {
                controlHintPanel.SetActive(false);
            }

            Time.timeScale = isPaused ? 0f : 1f; // 如果之前已经暂停了，保持暂停状态
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());

        // 确保音乐图标与状态一致
        UpdateMusicIcon();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());

        // 场景切换后，确保 UI 不被空格触发
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0.75f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        EventSystem.current?.SetSelectedGameObject(null);
    }

    // 暂停与恢复游戏
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Debug.Log("游戏" + (isPaused ? "已暂停" : "已恢复"));
    }

    // 重启当前关卡
    public void RestartLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LoadScene(currentScene);
    }

    // 返回主菜单
    public void ReturnToMainMenu()
    {
        LoadScene("StartScene");
    }

    // 打开/关闭设置面板
    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    // 音乐开启/关闭逻辑
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        if (musicSource != null)
        {
            if (isMusicMuted)
                musicSource.Pause();
            else
                musicSource.Play();
        }

        UpdateMusicIcon();
    }

    private void UpdateMusicIcon()
    {
        if (musicToggleButtonImage != null)
        {
            musicToggleButtonImage.sprite = isMusicMuted ? musicOffIcon : musicOnIcon;
        }
    }
}
