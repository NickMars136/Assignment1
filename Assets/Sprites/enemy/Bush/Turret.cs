using System;
using UnityEngine;

[RequireComponent(typeof(Shoot), typeof(Animator))]
public class Turret : BaseEnemy
{
    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 50f;

    private float nextShotTime = 2f;
    private bool isFiring = false;

    private Shoot shoot;

    public override void Start()
    {
        shoot = GetComponent<Shoot>();
        anim = GetComponent<Animator>();

        if (fireRate <= 0)
        {
            fireRate = 1.0f;
            Debug.LogWarning("TurretEnemy: Invalid fire rate - setting to 1 second.");
        }

        shoot.OnShotFired += ResetShotTimer;
    }

    private void ResetShotTimer(Vector2 velocity)
    {
        isFiring = false;
    }

    private void Update()
    {
        if (isFiring || Time.time < nextShotTime)
        {
            return;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Bush"))
        {
            isFiring = true;
            nextShotTime = Time.time + fireRate;
            anim.SetTrigger("Fire");
        }
    }

    private void OnDestroy()
    {
        if (shoot != null)
        {
            shoot.OnShotFired -= ResetShotTimer;
        }
    }
}