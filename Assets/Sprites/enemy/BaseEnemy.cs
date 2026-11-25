using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class BaseEnemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;
    [SerializeField] protected int maxHealth;
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            Debug.LogWarning("Max health is not set for " + gameObject.name);
            maxHealth = 3;
        }
        health = maxHealth;
    }
    public virtual void TakeDamage(int damage, DamageType damageType = DamageType.defualt)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetTrigger("Death");

            if (transform.parent != null )
            {
                Destroy(transform.parent.gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }
}

public enum DamageType
{
  defualt,
}