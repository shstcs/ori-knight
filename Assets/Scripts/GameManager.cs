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
    public static UnityAction OnHeal;
    public static UnityAction OnManaUp;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void CallDamage(int damage)
    {
        OnDamaged?.Invoke(damage);
    }
    public static void CallHeal()
    {
        OnHeal?.Invoke();
    }
    public static void CallManaUp()
    {
        OnManaUp?.Invoke();
    }

    public void OnStartButton()
    {
        SceneManager.LoadScene("inGame");
    }
}
