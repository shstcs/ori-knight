using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Manager>();
            }
            return instance;
        }
    }

    private static readonly GameManager gameManager = new();
    private static readonly UIManager uiManager = new();
    private static AudioManager audioManager;
    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject);
    }
    public static GameManager GameManager { get { return gameManager; } }
    public static UIManager UIManager { get { return uiManager; } }
    public static AudioManager AudioManager { get { return audioManager; } }
}
