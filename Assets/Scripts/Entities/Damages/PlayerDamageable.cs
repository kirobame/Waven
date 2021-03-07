using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] private Animator animator;

    protected override void OnDamageTaken() => animator.SetTrigger("isTakingDamage");
}