using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TurretTargeting : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float detectionRange = 8.0f;

    private Transform target;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FindTarget();

        if (target != null)
        {
            FaceTarget();
        }
    }

    private void FindTarget()
    {
        if (target == null)
        {
            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);

            if (targetObject != null)
            {
                target = targetObject.transform;
            }
        }

        if (target != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                target.position
            );

            if (distance > detectionRange)
            {
                target = null;
            }
        }
    }

    private void FaceTarget()
    {
        if (target.position.x < transform.position.x)
        {
            sr.flipX = false;
        }
        else if (target.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}