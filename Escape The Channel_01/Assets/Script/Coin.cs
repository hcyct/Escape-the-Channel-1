using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public Text coinCountText; // UI的硬币数量显示文本
    public AudioClip coinSound; // 硬币音效
    private static int coinCount = 0; // 硬币数量

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 播放音效
            PlayCoinSound();

            // 增加硬币数量
            coinCount++;

            // 更新UI中显示的硬币数量
            if (coinCountText != null)
            {
                coinCountText.text = "Coins: " + coinCount.ToString();
            }

            // 销毁硬币物体
            Destroy(gameObject);
        }
    }

    // 播放硬币音效的方法
    private void PlayCoinSound()
    {
        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
    }
}
