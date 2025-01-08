using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth = 100;
    public float speedModifier = 5.0f;
    public float jumpForce = 10f;
    public int extrajump = 1;

    [Header("Combat")]
    public int attackDmg = 10;
    public float criticalChance = 0.1f;
}
