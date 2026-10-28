using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
    
{
    #region Tuneable Parameters
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    #endregion
    #region Comp variables
    public Transform GroundCheckPos;
    public LayerMask groundlayer;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded;
    public Animator anim;

    #endregion
    #region Comp References
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityY <= 0f)
         { 
            isGrounded = Physics2D.OverlapCircle(GroundCheckPos.position, groundCheckRadius, groundlayer);
         }
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = moveX * moveSpeed;

        bool jumpPressed = Input.GetButtonDown("Jump");
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        Spriteflip(moveX);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveX", Mathf.Abs(moveX));
    }
    void Spriteflip(float moveX) => sr.flipX = (moveX < 0f);

}
