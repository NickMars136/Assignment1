using System;
using UnityEngine;

[RequireComponent(typeof(Shoot), typeof(Animator))]
public class Turret : BaseEnemy
{
    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 2.0f;
    [SerializeField] private float detectionRange = 8.0f;
    [SerializeField] private string targetTag = "Player";

    private float nextShotTime;

    private Shoot shoot;
    private Animator anim;
    private Transform target;
    private bool isFiring;

    public override void Start()
    {
        shoot = GetComponent<Shoot>();
        anim = GetComponent<Animator>();

        shoot.OnShotFired += ResetShotTimer;
    }

    private void Update()
    {
        FindTarget();

        if (target == null)
        {
            isFiring = false;
            anim.ResetTrigger("Fire");
            anim.Play("Idle", 0, 0f);
            return;
        }

        if (isFiring || Time.time < nextShotTime)
        {
            return;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle"))
        {
            isFiring = true;
            nextShotTime = Time.time + fireRate;
            anim.SetTrigger("Fire");
        }
    }

    private void FindTarget()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);

        if (targetObject == null)
        {
            target = null;
            return;
        }

        float distance = Vector2.Distance(
            transform.position,
            targetObject.transform.position
        );

        target = distance <= detectionRange
            ? targetObject.transform
            : null;
    }

    private void ResetShotTimer(Vector2 velocity)
    {
        isFiring = false;
    }

    private void OnDestroy()
    {
        if (shoot != null)
        {
            shoot.OnShotFired -= ResetShotTimer;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}