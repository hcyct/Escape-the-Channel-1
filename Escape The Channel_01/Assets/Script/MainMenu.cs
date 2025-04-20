using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // ��ʼ��Ϸ���л�����Ϸ����
    public void StartGame()
    {
        SceneManager.LoadScene("Level_01"); // ȷ�� "GameScene" ����ƥ��
    }
    public void Level_02()
    {
        SceneManager.LoadScene("Level_02"); // ȷ�� "GameScene" ����ƥ��
    }
    public void Level_03()
    {
        SceneManager.LoadScene("Level_03"); // ȷ�� "GameScene" ����ƥ��
    }


    // �˳���Ϸ
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �ڱ༭��ģʽ���˳�
#endif
    }
}
