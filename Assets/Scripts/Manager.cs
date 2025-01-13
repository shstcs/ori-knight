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
                Init();
            }
            return instance;
        }
    }
    private static void Init()
    {
        GameObject go = GameObject.Find("Manager");

        if (!go)
        {
            go = new GameObject("Manager");
            go.AddComponent<Manager>();
        }
        DontDestroyOnLoad(go);
        instance = go.GetComponent<Manager>();
    }

    private readonly GameManager gameManager = new();
    private readonly UIManager uiManager = new();
    private AudioManager audioManager;
    private void Awake()
    {
        Init();
        if(instance != this) Destroy(gameObject);
        audioManager = GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject);
    }
    public static GameManager GameManager => instance?.gameManager;
    public static UIManager UIManager => instance?.uiManager;
    public static AudioManager AudioManager => instance?.audioManager;
}
