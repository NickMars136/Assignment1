using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class BaseEnemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anima;
    protected int health;

    [SerializeField] protected int maxHealth = 3;

    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anima = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            maxHealth = 3;
            Debug.LogWarning("BaseEnemy: MaxHealth not defined - setting to a default 3");
        }

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage, Damagetype damagetype = Damagetype.Default)
    {
        health -= damage;
        if (health <= 0)
        {
            anima.SetTrigger("Death");
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject, 0.5f); // Delay the destruction to allow the death animation to play
            }
        }
    }
}
public enum Damagetype
{
    Default,
    JumpOn,
    Magic
}