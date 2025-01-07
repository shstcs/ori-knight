using System;
using System.Collections;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    Animator anim;
    public float moveSpeed = 1f;
    public float runSpeed = 1.5f;
    public int dir = 1;
    public float detectRange = 2f;
    private float wanderTime = 3f;
    private float curTime = 0f;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] private BoxCollider2D attackRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        attackRange.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        Wander();
        spriteRenderer.flipX = dir != 1;
        attackRange.transform.position = new Vector3(transform.position.x + (dir * 0.3f), transform.position.y, transform.position.z);
    }

    private void Wander()
    {
        Collider2D hitbox = Physics2D.OverlapCircle(transform.position,detectRange,playerLayer);
        if (hitbox != null && hitbox.CompareTag("Player"))
        {
            if(Vector2.Distance(transform.position,hitbox.transform.position) <= .5f)
            {
                rb.linearVelocity = Vector2.zero;
                Attack();
            }
            else
            {
                dir = hitbox.transform.position.x >= transform.position.x ? 1 : -1;
                rb.linearVelocity = new Vector2(dir * runSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            curTime += Time.deltaTime;
            if (curTime > wanderTime)
            {
                curTime = 0;
                dir = -1 * dir;
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Debug.Log(curTime);
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void SetAttackRange() { attackRange.enabled = true; }
    public void UnsetAttackRange() { attackRange.enabled = false; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //TODO 적 피해 구현
            Debug.Log("Damaged!!!");
        }
    }
}
