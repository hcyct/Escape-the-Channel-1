using UnityEngine;
using UnityEngine.UI;

public class MuteMusicToggle : MonoBehaviour
{
    [Header("图标设置")]
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    [Header("目标组件")]
    public Image buttonImage; // 显示图标的 UI Image
    public AudioSource targetAudioSource; // 要控制的音乐播放器

    private bool isMuted = false;

    void Start()
    {
        // 初始化状态（音乐默认播放）
        if (targetAudioSource == null)
        {
            targetAudioSource = FindObjectOfType<AudioSource>(); // 自动获取
        }

        UpdateButtonIcon();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            targetAudioSource.Pause(); // 停止音乐
        }
        else
        {
            targetAudioSource.Play(); // 继续播放
        }

        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? musicOffIcon : musicOnIcon;
        }
    }
}
