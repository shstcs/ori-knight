using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D hurtLight;
    private void Start()
    {
        Manager.GameManager.OnHPone += () => ChangeMoribund(true);
        Manager.GameManager.OnHPRestore += () => ChangeMoribund(false);
        hurtLight.enabled = false;
    }

    private void ChangeMoribund(bool state)
    {
        globalLight.enabled = !state;
        hurtLight.enabled = state;
    }
}
