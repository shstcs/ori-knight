using UnityEngine;

public class Item_Heal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().playerSO.canHeal = true;
            Destroy(gameObject);
        }
    }
}
