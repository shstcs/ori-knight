using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IAttackable
{
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    private SpriteRenderer parrySprite;
    private PolygonCollider2D polygonCollider;
    private Animator anim;
    private ParticleSystem Healparticle;
    private ParticleSystem Dashparticle;

    public PlayerSO playerSO;
    [SerializeField] private BoxCollider2D attackRange;
    private Vector2 dir = Vector2.zero;
    private int jumpCount = 0;
    private bool isGrounded = true;
    private bool isTouchingWall = false;
    public bool isDashing = false;
    private bool isHPone = false;
    public bool isParrying = false;
    private int curDamage;
    private int curHP;
    private int curMP;

    private int AttackManaGainCount = 0;

    #region LifeCycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        parrySprite = GetComponentsInChildren<SpriteRenderer>()[1];
        polygonCollider = GetComponent<PolygonCollider2D>();
        Healparticle = GetComponentsInChildren<ParticleSystem>()[0];
        Dashparticle = GetComponentsInChildren<ParticleSystem>()[1];
    }

    private void Start()
    {
        attackRange.enabled = false;
        curDamage = playerSO.attackDmg;
        curHP = playerSO.maxHealth;
        curMP = playerSO.maxMana;
        parrySprite.enabled = false;
        Manager.GameManager.OnHeal += PerformHeal;
        Manager.GameManager.OnDash += StartDashCoroutine;
        Manager.GameManager.OnHPone += () => Manager.AudioManager.PlayBreathLoop(true);
        Manager.GameManager.OnHPRestore += () => Manager.AudioManager.PlayBreathLoop(false);
    }

    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Floor"));
        isTouchingWall = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Wall"));
        float clampedY = isTouchingWall ? Mathf.Clamp(rb.linearVelocity.y, -3f, float.MaxValue) : rb.linearVelocity.y;
        float moveValue = isParrying ? 0 : dir.x;
        rb.linearVelocity = new Vector2(moveValue * playerSO.speedModifier, clampedY);

        int movedir = dir.x > 0 ? 1 : -1;
        if (dir.x != 0 && !isParrying) playerSprite.flipX = dir.x < 0;
        attackRange.transform.position = new Vector2(transform.position.x + (0.35f * movedir), transform.position.y);
        Manager.AudioManager.PlayFootprintLoop(dir.x != 0);
    }

    private void LateUpdate()
    {
        SetCollider();
    }
    #endregion

    #region Perform
    public void PerformMove(Vector2 input)
    {
        dir.x = input.x;
        anim.SetBool("Run", dir.x != 0);
    }
    public void PerformJump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerSO.jumpForce);
            Debug.Log("Jump Perform");
        }
        else if (jumpCount < playerSO.extrajump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerSO.jumpForce);
            jumpCount++;
        }
    }
    public void PerformAttack()
    {
        if (Manager.GameManager.skillCooltimes[(int)Cooltimes.Attack])
        {
            anim.SetTrigger("Attack");
            Manager.AudioManager.PlaySFX(SoundEffect.PlayerAttack);
            StartCoroutine(Cooldown(Cooltimes.Attack, playerSO.attackSpeed));
        }
    }
    private void PerformHeal()
    {
        if (Manager.GameManager.skillCooltimes[(int)Cooltimes.Heal])
        {
            if (curMP > 0)
            {
                curMP--;
                if (curHP < playerSO.maxHealth) curHP++;
                if (isHPone)
                {
                    isHPone = false;
                    Manager.GameManager.CallHPRestore();
                }
                StartCoroutine(Cooldown(Cooltimes.Heal, playerSO.HealCooltime));
                Healparticle.Play();
            }
        }
    }
    private IEnumerator PerformDash()
    {
        int dashDir = playerSprite.flipX ? -1 : 1;
        float range = playerSO.DashRange;
        isDashing = true;
        float height = transform.position.y;
        while (range >= 0)
        {
            transform.position = new Vector3(transform.position.x + (0.5f * dashDir), height, transform.position.z);
            range -= 0.5f;
            Dashparticle.Play();
            yield return null;
        }
        isDashing = false;
    }
    public void PerformParry()
    {
        if (Manager.GameManager.skillCooltimes[(int)Cooltimes.Parry])
        {
            Debug.Log("Parry!");
            anim.SetTrigger("Parry");
            StartCoroutine(Parrying());
            StartCoroutine(Cooldown(Cooltimes.Parry, 2f));
        }
    }
    #endregion

    private IEnumerator Cooldown(Cooltimes skill, float cooltime)
    {
        Manager.GameManager.skillCooltimes[(int)skill] = false;
        yield return new WaitForSeconds(cooltime);
        Manager.GameManager.skillCooltimes[(int)skill] = true;
    }

    private void StartDashCoroutine()
    {
        if (Manager.GameManager.skillCooltimes[(int)Cooltimes.Dash])
        {
            Debug.Log(Manager.GameManager.skillCooltimes[(int)Cooltimes.Dash]);
            StartCoroutine(PerformDash());
            StartCoroutine(Cooldown(Cooltimes.Dash, playerSO.DashCooltime));
        }
    }

    private IEnumerator Parrying()
    {
        isParrying = true;
        parrySprite.enabled = true;
        anim.SetBool("isParry",true);

        yield return new WaitForSeconds(0.5f);

        isParrying = false;
        parrySprite.enabled = false;
        anim.SetBool("isParry", false);
    }

    public (int, int) ShowHPMP()
    {
        return (curHP, curMP);
    }
    public void SetAttackRange() { attackRange.enabled = true; }
    public void UnsetAttackRange() { attackRange.enabled = false; }

    private void SetCollider()
    {
        polygonCollider.pathCount = playerSprite.sprite.GetPhysicsShapeCount();
        List<Vector2> path = new List<Vector2>();
        playerSprite.sprite.GetPhysicsShape(0, path);
        polygonCollider.SetPath(0, path);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || (collision.gameObject.CompareTag("Wall") && playerSO.canClimb))
        {
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            collision.gameObject.GetComponent<MonsterControl>().TakeDamage(curDamage);
            collision.gameObject.GetComponent<MonsterControl>().Knockback(transform.position);
            AttackManaGainCount++;
            if (AttackManaGainCount >= playerSO.manaRegainCount && curMP < playerSO.maxMana)
            {
                curMP++;
                Manager.GameManager.CallManaUp();
                AttackManaGainCount = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isParrying)
        {
            StartCoroutine(ParrySlow());
        }
        else
        {
            Debug.Log("Player Damaged : " + damage);
            curHP -= damage;
            if (curHP < 0) curHP = 0;
            if (curHP == 0)
            {
                // TODO. 게임오버 창 띄우기
            }
            else if (curHP == 1)
            {
                Manager.GameManager.CallHPOne();
                isHPone = true;
            }
            anim.SetTrigger("Hurt");
            StartCoroutine(DamagedColorChange());
        }
    }
    private IEnumerator ParrySlow()
    {
        Time.timeScale = 0.5f;
        Camera.main.orthographicSize = 4.5f;
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 1f;
        Camera.main.orthographicSize = 5f;
    }
    public void Knockback(Vector3 pos)
    {
        rb.AddForce(new Vector2(0, 3f), ForceMode2D.Impulse);
    }
    public IEnumerator DamagedColorChange()
    {
        Color original = playerSprite.color;
        playerSprite.color = new Color(.5f, .5f, .5f);
        yield return new WaitForSeconds(0.2f);
        playerSprite.color = original;
    }
}
