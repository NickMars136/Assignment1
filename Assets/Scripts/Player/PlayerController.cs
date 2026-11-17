using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour

{
    #region Tuneable Parameters
    public float moveSpeed = 5f;
    private float jumpForce = 7.5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    private bool isDashing;
    [SerializeField] private int maxLives = 4;
    private int lives = 3;

    public void setLives(int lifeValue)
    {
        if (lifeValue > maxLives)
        {
            lives = maxLives;
        }
        else if (lifeValue <= 0)
        {
            lives = 0;
        }
        else if (lives < lifeValue)
        {
            lives = lifeValue;
        }

        Debug.Log($"Lives: {lives}");

    }
    public int GetLives()
    {
        return lives;
    }
    #endregion
    #region Comp variables
    public Transform GroundCheckPos;
    public LayerMask groundlayer;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded;
    public Animator anim;
    private float horizontalInput = 5f;
    #endregion
    #region Comp References
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    #endregion
    private float currentPowerupDuration = 0;
    public float initialPowerupDuration = 10;
    private float intialJumpForce = 7.5f;
    private Coroutine JumpForceCoroutine = null;

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

        if (!isDashing)
        {
            rb.linearVelocityX = moveX * moveSpeed;
        }

        bool jumpPressed = Input.GetButtonDown("Jump");
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("isAtk");
        }
        Spriteflip(moveX);
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveX", Mathf.Abs(moveX));
    }
    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        anim.SetTrigger("isDash");

        float direction = sr.flipX ? -1f : 1f;

        rb.linearVelocity = new Vector2(direction * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }
    void Spriteflip(float moveX) => sr.flipX = (moveX < 0f);

    public void JumpForceChange()
    {
        if (JumpForceCoroutine != null)
        {
            StopCoroutine(JumpForceCoroutine);
            JumpForceCoroutine = null;
            jumpForce = intialJumpForce;
        }
        JumpForceCoroutine = StartCoroutine(JumpForceChangeCoroutine());
    }
    IEnumerator JumpForceChangeCoroutine()
    {
        currentPowerupDuration = initialPowerupDuration + currentPowerupDuration;
        jumpForce = intialJumpForce * 1.5f;

        while (currentPowerupDuration > 0)
        {
            currentPowerupDuration -= Time.deltaTime;
            Debug.Log($"Current Powerup Duration: {currentPowerupDuration}");
            yield return null;
        }
        jumpForce = intialJumpForce;
        JumpForceCoroutine = null;
        currentPowerupDuration = 0;
        Debug.Log($"Jump Force: {jumpForce} | Baseline: {intialJumpForce}");
    }
}