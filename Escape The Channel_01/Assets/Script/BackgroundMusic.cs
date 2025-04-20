using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    [Header("�����б�")]
    public AudioClip[] musicClips;
    private int currentClipIndex = 0;

    [Header("���뵭������")]
    public float fadeDuration = 1.5f;
    public float targetVolume = 0.8f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            if (musicClips.Length > 0)
            {
                audioSource.clip = musicClips[currentClipIndex];
                audioSource.volume = 0;
                audioSource.Play();
                StartCoroutine(FadeIn());
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �ⲿ��ť���ã��л���һ�׸�
    /// </summary>
    public void NextTrack()
    {
        StartCoroutine(SwitchTrackWithFade());
    }

    private IEnumerator SwitchTrackWithFade()
    {
        yield return StartCoroutine(FadeOut());

        // �л���һ��
        currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;
    }

    private IEnumerator FadeIn()
    {
        float startVolume = 0f;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}
