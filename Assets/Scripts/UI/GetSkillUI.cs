using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetSkillUI : MonoBehaviour
{
    [SerializeField] private Sprite[] Icons;
    [SerializeField] private Image SkillImage;
    [SerializeField] private TMP_Text skillText;
    private string[] explains = { "You can double jump!", "press H to heal.", "press left shift to dash.", "You can jump at wall" };
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ExitUI()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
        if(Manager.GameManager.getItemNum != 99)
        {
            Debug.Log(Manager.GameManager.getItemNum);
            SkillImage.sprite = Icons[Manager.GameManager.getItemNum];
            skillText.text = explains[Manager.GameManager.getItemNum];
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
