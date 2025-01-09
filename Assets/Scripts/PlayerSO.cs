using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth = 3;
    public int maxMana = 3;
    public float speedModifier = 5.0f;
    public float jumpForce = 10f;
    public int extrajump = 0;
    public bool canHeal = false;
    public bool canDash = false;
    public bool canClimb = false;

    [Header("Combat")]
    public int attackDmg = 10;
    public float attackSpeed = 0.5f;
    public float criticalChance = 0.1f;
    public int manaRegainCount = 10;
    public float DashRange = 3f;
}
