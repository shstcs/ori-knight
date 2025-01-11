using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public void GameStartButton()
    {
        SceneManager.LoadScene("inGame");
    }
    public void ExitButton()
    {
        //게임 종료
    }
}
