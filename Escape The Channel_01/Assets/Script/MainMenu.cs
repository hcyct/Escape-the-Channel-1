using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 开始游戏，切换到游戏场景
    public void StartGame()
    {
        SceneManager.LoadScene("Level_01"); // 确保 "GameScene" 名称匹配
    }
    public void Level_02()
    {
        SceneManager.LoadScene("Level_02"); // 确保 "GameScene" 名称匹配
    }
    public void Level_03()
    {
        SceneManager.LoadScene("Level_03"); // 确保 "GameScene" 名称匹配
    }


    // 退出游戏
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器模式下退出
#endif
    }
}
