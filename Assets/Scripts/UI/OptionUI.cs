using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Slider volume;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ExitUI()
    {
        gameObject.SetActive(false);
    }
    public void VolumeControl()
    {
        Manager.GameManager.volume = (int)volume.value;
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
        if(Manager.GameManager != null) volume.value = Manager.GameManager.volume;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
