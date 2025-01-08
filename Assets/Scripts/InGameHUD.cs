using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    [SerializeField] private Image[] heart;
    [SerializeField] private Image[] mana;
    [SerializeField] private GameObject player;
    private int curHP;
    private int curMP;

    private void Awake()
    {
        GameManager.OnDamaged += DecreaseHP;
    }
    void Start()
    {
        curHP = player.GetComponent<PlayerControl>().playerSO.maxHealth;
        curMP = player.GetComponent<PlayerControl>().playerSO.maxMana;
    }

    public void DecreaseHP(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            if (curHP < i) break;
            heart[curHP - i].enabled = false;
        }
        curHP -= damage;
    }
}
