using Unity.VisualScripting;
using UnityEngine;

public class Item_DoubleJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().playerSO.extrajump++;
            Destroy(gameObject);
        }
    }
}
