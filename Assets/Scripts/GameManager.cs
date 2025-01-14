using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UnityAction<int> OnDamaged;
    public UnityAction OnHeal;
    public UnityAction OnManaUp;
    public UnityAction OnDash;
    public UnityAction OnGetHealItem;
    public UnityAction OnGetDashItem;
    public UnityAction OnGetItem;
    public UnityAction OnOption;
    public UnityAction OnClear;
    public UnityAction OnHPone;
    public UnityAction OnHPRestore;

    public int getItemNum = 99;
    public int volume = 50;
    public bool[] skillCooltimes = new bool[4] { true, true, true, true };

    public void CallDamage(int damage)
    {
        OnDamaged?.Invoke(damage);
    }
    public void CallHeal()
    {
        OnHeal?.Invoke();
    }
    public void CallManaUp()
    {
        OnManaUp?.Invoke();
    }

    public void CallDash()
    {
        OnDash?.Invoke();
    }
    public void CallGetHealItem()
    {
        OnGetHealItem?.Invoke();
    }
    public void CallGetDashItem()
    {
        OnGetDashItem?.Invoke();
    }
    public void CallGetItem()
    {
        OnGetItem?.Invoke();
    }
    public void CallOnOption()
    {
        OnOption?.Invoke();
    }
    public void CallClear()
    {
        OnClear?.Invoke();
    }
    public void CallHPOne()
    {
        OnHPone?.Invoke();
    }
    public void CallHPRestore()
    {
        OnHPRestore?.Invoke();
    }
    public void OnStartButton()
    {
        SceneManager.LoadScene("inGame");
    }
}