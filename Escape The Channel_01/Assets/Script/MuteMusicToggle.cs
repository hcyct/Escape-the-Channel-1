using UnityEngine;
using UnityEngine.UI;

public class MuteMusicToggle : MonoBehaviour
{
    [Header("ͼ������")]
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    [Header("Ŀ�����")]
    public Image buttonImage; // ��ʾͼ��� UI Image
    public AudioSource targetAudioSource; // Ҫ���Ƶ����ֲ�����

    private bool isMuted = false;

    void Start()
    {
        // ��ʼ��״̬������Ĭ�ϲ��ţ�
        if (targetAudioSource == null)
        {
            targetAudioSource = FindObjectOfType<AudioSource>(); // �Զ���ȡ
        }

        UpdateButtonIcon();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            targetAudioSource.Pause(); // ֹͣ����
        }
        else
        {
            targetAudioSource.Play(); // ��������
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
