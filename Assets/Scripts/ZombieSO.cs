using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSO", menuName = "Scriptable Objects/ZombieSO")]
public class ZombieSO : ScriptableObject
{
    [Header("Stats")]
    public int maxHP = 30;
    public float moveSpeed = 1f;
    public float runSpeed = 1.5f;
    public float detectRange = 2f;
    public float wanderTime = 3f;

    [Header("Combat")]
    public float attackPreDelay = 0.2f;
    public float attackAfterDelay = 1f;
    public int attackDmg = 1;
    public SoundEffect attackSound = SoundEffect.Zombie1;
}
