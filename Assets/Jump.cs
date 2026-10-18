using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    Rigidbody2D rb;
    private InputSystem_Actions actions;
    float move;
    public float jumpforce;
    public float speed;
    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    bool isGrounded;
    public Animator animator;
    public float attackCooldown;
    private float nextAttackTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        actions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        actions.Player.Enable(); //action map calledp player so that actions can be used
        actions.Player.Jump.performed += Jumping;
        actions.Player.Move.performed += Movement;

        actions.Player.Jump.canceled += Jumping;
        actions.Player.Move.canceled += Movement;      
    }

    void OnDisable()
    {
        actions.Player.Disable(); 
        actions.Player.Jump.performed -= Jumping;
        actions.Player.Move.performed -= Movement;
        
    }

    void Movement(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
    }

    void Jumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed&&isGrounded){    
            rb.linearVelocityY = jumpforce;
            animator.SetTrigger("Jump");
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = move*speed;
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position,groundCheckRadius,groundLayer); //is it grounded lol

        animator.SetBool("AnimState",Mathf.Abs(move) > 0.1f);
        animator.SetBool("Grounded",isGrounded);
        animator.SetFloat("AirSpeedY",rb.linearVelocityY);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack1");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);

    }

}
