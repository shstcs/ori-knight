using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        Manager.GameManager.OnClear += GameClear;
    }
    public void GameClear()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ToStartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Start");
    }
}
