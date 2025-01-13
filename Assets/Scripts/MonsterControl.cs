using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterControl : MonoBehaviour, IAttackable, IKnockbackable
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Light2D attackLight;

    [SerializeField] private ZombieSO zombieSO;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D attackRange;

    private int dir = 1;
    private float curTime = 0f;
    private bool isAttacking = false;
    private bool isDead = false;
    private float atkTime = 0;
    private int curHP;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        attackLight = GetComponentInChildren<Light2D>();
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
        if (!isAttacking && !isDead)
        {
            Collider2D hitbox = Physics2D.OverlapCircle(transform.position, zombieSO.detectRange, playerLayer);
            if (hitbox != null && hitbox.CompareTag("Player"))
            {
                anim.SetBool("Move", true);
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
            else if (!isDead)
            {
                anim.SetBool("Move", true);
                rb.linearVelocity = new Vector2(dir * zombieSO.moveSpeed, rb.linearVelocity.y);
                curTime += Time.deltaTime;
                if (curTime > zombieSO.wanderTime)
                {
                    curTime = 0;
                    dir = -1 * dir;
                }
            }
            else
            {
                anim.SetBool("Move", false);
                rb.linearVelocity = Vector2.zero;
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
        StartCoroutine(OnAttackLight(zombieSO.attackPreDelay));
        yield return new WaitForSeconds(zombieSO.attackPreDelay);
        Manager.AudioManager.PlaySFX(zombieSO.attackSound);
        anim.SetTrigger("Attack");
    }

    public IEnumerator OnAttackLight(float time)
    {
        while(attackLight.intensity < 100)
        {
            attackLight.intensity += 100 * Time.deltaTime / (time + 0.2f);
            Debug.Log(attackLight.intensity);
            yield return null;
        }
        attackLight.intensity = 0;
    }
    public void SetAttackRange() { attackRange.enabled = true; }
    public void UnsetAttackRange() { attackRange.enabled = false; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerControl>().isDashing)
        {
            if (!collision.GetComponent<PlayerControl>().isParrying)
            {
                Manager.GameManager.CallDamage(zombieSO.attackDmg);
                collision.gameObject.GetComponent<PlayerControl>().Knockback(transform.position);
            }
            collision.gameObject.GetComponent<PlayerControl>().TakeDamage(zombieSO.attackDmg);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Monster Damaged : " + damage);
        curHP -= damage;
        anim.SetTrigger("Hurt");
        anim.SetBool("isHurted", true);
        StartCoroutine(DamagedColorChange());
        if (curHP <= 0)
        {
            anim.SetTrigger("Die");
            anim.SetBool("isDead", true);
            isDead = true;
        }
    }

    public void Dead()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Knockback(Vector3 pos)
    {
        rb.AddForce(new Vector2(0, 3f), ForceMode2D.Impulse);
    }

    public IEnumerator DamagedColorChange()
    {
        Color original = spriteRenderer.color;
        spriteRenderer.color = new Color(.5f, .5f, .5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = original;
        anim.SetBool("isHurted", false);
    }
}
