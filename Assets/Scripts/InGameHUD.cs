using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Image[] heart;
    [SerializeField] private Image[] mana;
    [SerializeField] private GameObject HealIcon;
    [SerializeField] private GameObject DashIcon;
    private Image HealCooltimeIcon;
    private Image DashCooltimeIcon;
    private int curHP;
    private int curMP;

    private void Awake()
    {
        Manager.GameManager.OnDamaged += DecreaseHP;
        Manager.GameManager.OnHeal += HealingUI;
        Manager.GameManager.OnManaUp += ManaGain;
        Manager.GameManager.OnDash += UseDashSkill;
        Manager.GameManager.OnGetHealItem += () => HealIcon.SetActive(true);
        Manager.GameManager.OnGetDashItem += () => DashIcon.SetActive(true);

        HealCooltimeIcon = HealIcon.GetComponentsInChildren<Image>()[1];
        DashCooltimeIcon = DashIcon.GetComponentsInChildren<Image>()[1];
    }
    private void Start()
    {
        HealCooltimeIcon.fillAmount = 0;
        DashCooltimeIcon.fillAmount = 0;
        HealIcon.SetActive(false);
        DashIcon.SetActive(false);
    }

    public void DecreaseHP(int damage)
    {
        curHP = player.GetComponent<PlayerControl>().ShowHPMP().Item1;

        for (int i = 1; i <= damage; i++)
        {
            if (curHP < i) break;
            heart[curHP - i].enabled = false;
        }
    }
    public void HealingUI()
    {
        if (Manager.GameManager.skillCooltimes[(int)Cooltimes.Heal])
        {
            ChangeHealStatus();
            ShowHealCooldown();
        }
    }
    public void ChangeHealStatus()
    {
        (curHP, curMP) = player.GetComponent<PlayerControl>().ShowHPMP();

        heart[curHP].enabled = true;
        mana[curMP - 1].enabled = false;
    }

    public void ManaGain()
    {
        curMP = player.GetComponent<PlayerControl>().ShowHPMP().Item2;

        mana[curMP-1].enabled = true;
    }
    private void ShowHealCooldown()
    {
        StartCoroutine(CoolDown(HealCooltimeIcon, player.GetComponent<PlayerControl>().playerSO.HealCooltime));
    }
    private void UseDashSkill()
    {
        if (Manager.GameManager.skillCooltimes[(int)Cooltimes.Dash])
            StartCoroutine(CoolDown(DashCooltimeIcon, player.GetComponent<PlayerControl>().playerSO.DashCooltime));
    }
    private IEnumerator CoolDown(Image skill, float cooltime)
    {
        skill.fillAmount = 1;
        while(skill.fillAmount > 0)
        {
            skill.fillAmount -= Time.deltaTime / cooltime;
            yield return null;
        }
    }
}
