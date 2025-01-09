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
        GameManager.OnHeal += Healing;
        GameManager.OnManaUp += ManaGain;
    }

    public void DecreaseHP(int damage)
    {
        curHP = player.GetComponent<PlayerControl>().showHPMP().Item1;

        for (int i = 1; i <= damage; i++)
        {
            if (curHP < i) break;
            heart[curHP - i].enabled = false;
        }
    }

    public void Healing()
    {
        (curHP, curMP) = player.GetComponent<PlayerControl>().showHPMP();

        heart[curHP].enabled = true;
        mana[curMP - 1].enabled = false;
    }

    public void ManaGain()
    {
        curMP = player.GetComponent<PlayerControl>().showHPMP().Item2;

        mana[curMP-1].enabled = true;
    }
}
