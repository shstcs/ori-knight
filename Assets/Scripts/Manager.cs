using System;
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
        Debug.Log(go);
        if (!go)
        {
            go = new GameObject("Manager");
            go.AddComponent<Manager>();
        }

        instance = go.GetComponent<Manager>();
    }

    private GameManager gameManager;
    private UIManager uiManager;
    private AudioManager audioManager;
    private void Awake()
    {
        instance = this;
        gameManager = gameObject.AddComponent<GameManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        audioManager = GetComponent<AudioManager>();
        Debug.Log(uiManager.gameObject);
    }
    public static GameManager GameManager => instance?.gameManager;
    public static UIManager UIManager => instance?.uiManager;
    public static AudioManager AudioManager => instance?.audioManager;
}
