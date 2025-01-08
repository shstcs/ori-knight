using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;
        }
    }

    public static UnityAction<int> OnDamaged;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public static void CallDamage(int damage)
    {
        OnDamaged?.Invoke(damage);
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("inGame");
    }
}
