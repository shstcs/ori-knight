using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HurtLightBlink : MonoBehaviour
{
    private new Light2D light;
    private float period = 1.5f;
    private float cnt = 0;
    private int dir = 1;
    [SerializeField] private GameObject player;
    void Start()
    {
        light = GetComponent<Light2D>();
        light.intensity = 0.5f;
    }

    void Update()
    {
        if(cnt > period)
        {
            dir *= -1; cnt = 0;
        }
        else
        {
            light.intensity += Time.deltaTime * dir;
            cnt += Time.deltaTime;
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
