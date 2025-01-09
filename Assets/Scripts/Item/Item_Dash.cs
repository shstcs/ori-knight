using UnityEngine;

public class Item_Dash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().playerSO.canDash = true;
            Destroy(gameObject);
        }
    }
}
