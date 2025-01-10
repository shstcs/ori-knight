using UnityEngine;

public class Item_Climb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().playerSO.canClimb = true;
            Manager.GameManager.getItemNum = (int)Items.climb;
            Manager.GameManager.CallGetItem();
            Destroy(gameObject);
        }
    }
}
