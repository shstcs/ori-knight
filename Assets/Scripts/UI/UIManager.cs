using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject optionUI;
    public GameObject getItemUI;
    private void Start()
    {
        Manager.GameManager.OnGetItem += ShowGetItemUI;
        Manager.GameManager.OnOption += ShowOptionUI;
    }
    public void ShowGetItemUI()
    {
        getItemUI.SetActive(true);
    }
    public void ShowOptionUI()
    {
        optionUI.SetActive(true);
    }
}
