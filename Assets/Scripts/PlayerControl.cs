using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 dir = Vector2.zero;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;
    Animator anim;
    public float speedModifier = 5.0f;
    public float jumpForce = 10f;
    private int jumpCount = 0;
    public int extrajump = 1;
    private bool isGrounded = true;
    [SerializeField] private BoxCollider2D attackRange;

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
    }

    private void Update()
    {
        Camera.main.transform.position = new Vector3 (transform.position.x, transform.position.y, Camera.main.transform.position.z);
        
        rb.linearVelocity = new Vector2(dir.x * speedModifier, rb.linearVelocity.y);
        if(dir.x > 0)
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

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            dir.x = input.x;
            anim.SetBool("Run",true);
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
            if(isGrounded)
            {
                isGrounded = false;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if(jumpCount < extrajump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount++;
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anim.SetTrigger("Attack");
        }
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
            //TODO 적 피해 구현
            Debug.Log("Damaged!");
        }
    }
}
