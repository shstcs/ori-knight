using System;
using System.Collections;
using UnityEngine;

public class MonsterControl : MonoBehaviour, IAttackable
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    [SerializeField] private ZombieSO zombieSO;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D attackRange;

    private int dir = 1;
    private float curTime = 0f;
    private bool isAttacking = false;
    private float atkTime = 0;
    private int curHP;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        attackRange.enabled = false;
        curHP = zombieSO.maxHP;
    }

    void Update()
    {
        Wander();
        spriteRenderer.flipX = dir != 1;
        attackRange.transform.position = new Vector3(transform.position.x + (dir * 0.3f), transform.position.y, transform.position.z);
    }

    private void Wander()
    {
        if (!isAttacking)
        {
            Collider2D hitbox = Physics2D.OverlapCircle(transform.position, zombieSO.detectRange, playerLayer);
            if (hitbox != null && hitbox.CompareTag("Player"))
            {
                if (Vector2.Distance(transform.position, hitbox.transform.position) <= .75f)
                {

                    isAttacking = true;
                    StartCoroutine(Attack());
                }
                else
                {
                    dir = hitbox.transform.position.x >= transform.position.x ? 1 : -1;
                    rb.linearVelocity = new Vector2(dir * zombieSO.runSpeed, rb.linearVelocity.y);
                }
            }
            else
            {
                rb.linearVelocity = new Vector2(dir * zombieSO.moveSpeed, rb.linearVelocity.y);
                curTime += Time.deltaTime;
                if (curTime > zombieSO.wanderTime)
                {
                    curTime = 0;
                    dir = -1 * dir;
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            atkTime += Time.deltaTime;
            if (atkTime > zombieSO.attackAfterDelay)
            {
                atkTime = 0;
                isAttacking = false;
            }
        }
    }
    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(zombieSO.attackPreDelay);
        anim.SetTrigger("Attack");
    }
    public void SetAttackRange() { attackRange.enabled = true; }
    public void UnsetAttackRange() { attackRange.enabled = false; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControl>().TakeDamage(zombieSO.attackDmg);
            Debug.Log("Player Damaged!");
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Monster Damaged : " + damage);
        curHP -= damage; 

        if (curHP <= 0)
        {

        }
    }
}
