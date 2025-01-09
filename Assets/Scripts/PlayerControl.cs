using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class PlayerControl : MonoBehaviour, IAttackable
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    private Animator anim;

    public PlayerSO playerSO;
    [SerializeField] private BoxCollider2D attackRange;
    private Vector2 dir = Vector2.zero;
    private int jumpCount = 0;
    private bool isGrounded = true;
    private bool isTouchingWall = false;
    public bool isDashing = false;
    private int curDamage;
    private int curHP;
    private int curMP;
    private bool canAttack = true;
    public int AttackManaGainCount = 0;

    #region LifeCycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        attackRange.enabled = false;
        curDamage = playerSO.attackDmg;
        curHP = playerSO.maxHealth;
        curMP = playerSO.maxMana;
        GameManager.OnHeal += PerformHeal;
    }

    private void Update()
    {
        isTouchingWall = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Wall"));
        float clampedY = isTouchingWall ? Mathf.Clamp(rb.linearVelocity.y, -3f, float.MaxValue) : rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(dir.x * playerSO.speedModifier, clampedY);

        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
            attackRange.transform.position = new Vector2(transform.position.x + 0.35f, transform.position.y);
        }
        else if (dir.x < 0)
        {
            spriteRenderer.flipX = true;
            attackRange.transform.position = new Vector2(transform.position.x - 0.35f, transform.position.y);
        }
    }

    private void LateUpdate()
    {
        SetCollider();
    }
    #endregion

    #region InputAction
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            dir.x = input.x;
            anim.SetBool("Run", true);
        }
        else
        {
            dir = Vector2.zero;
            anim.SetBool("Run", false);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                isGrounded = false;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerSO.jumpForce);
            }
            else if (jumpCount < playerSO.extrajump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerSO.jumpForce);
                jumpCount++;
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(AttackCooldown());
        }
    }

    public void Heal(InputAction.CallbackContext context)
    {
        if (context.started && playerSO.canHeal && curHP < playerSO.maxHealth)
        {
            GameManager.CallHeal();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started && playerSO.canDash)
        {
            Debug.Log("Dash");
            StartCoroutine(PerformDash());
        }
    }
    #endregion

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(playerSO.attackSpeed);
        canAttack = true;
    }

    private void PerformHeal()
    {
        if (curMP > 0)
        {
            curMP--;
            if (curHP < playerSO.maxHealth) curHP++;
        }
    }

    public void HPchange(int num)
    {
        curHP += num;
    }
    public void MPchange(int num)
    {
        curMP += num;
    }
    public (int, int) showHPMP()
    {
        return (curHP, curMP);
    }

    private IEnumerator PerformDash()
    {
        int dashDir = spriteRenderer.flipX ? -1 : 1;
        float range = playerSO.DashRange;
        isDashing = true;
        while (range >= 0)
        {
            transform.position = new Vector3(transform.position.x + (0.5f * dashDir), transform.position.y, transform.position.z);
            range -= 0.5f;
            yield return null;
        }
        isDashing = false;
    }

    public void SetAttackRange() { attackRange.enabled = true; }
    public void UnsetAttackRange() { attackRange.enabled = false; }

    private void SetCollider()
    {
        polygonCollider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
        List<Vector2> path = new List<Vector2>();
        spriteRenderer.sprite.GetPhysicsShape(0, path);
        polygonCollider.SetPath(0, path);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
        if (collision.gameObject.CompareTag("Wall") && playerSO.canClimb)
        {
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
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
                GameManager.CallManaUp();
                AttackManaGainCount = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player Damaged : " + damage);
        curHP -= damage;
        anim.SetTrigger("Hurt");
        StartCoroutine(DamagedColorChange());
        if (curHP <= 0)
        {
            // TODO. 게임오버 창 띄우기
        }
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
    }
}
