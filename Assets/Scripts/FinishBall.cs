using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                Manager.GameManager.CallClear();
            }
        }
    }
}
