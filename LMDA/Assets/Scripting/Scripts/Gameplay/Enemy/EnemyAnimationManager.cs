using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    public Animator Animator;
    void Start()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    public void Dead()
    {
        Animator.SetBool("Dead", true);
    }

    public void Attack()
    {
        Animator.Play("Attack");
    }

    public void Hit()
    {
        Animator.Play("Hit");
    }
    
}
