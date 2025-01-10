using UnityEngine;

public class Background : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y,transform.position.z);
    }
}
