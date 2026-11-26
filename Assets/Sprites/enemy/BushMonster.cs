using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BushMonster : BaseEnemy
{
    [SerializeField] private float xVel = 2.0f;
    private Rigidbody2D rb;
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Bush"))
        {
            rb.linearVelocityX = sr.flipX ? xVel : -xVel;
        }
    }
    public override void TakeDamage(int damage, DamageType damageType = DamageType.defualt)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Death"))
        {
            return;
        }
        if (damageType == DamageType.defualt)
        {
            anim.SetTrigger("Hurt");
            Destroy(transform.parent.gameObject, 0.5f);
        }
        base.TakeDamage(damage, damageType);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            sr.flipX = !sr.flipX;
        }
    }
}
