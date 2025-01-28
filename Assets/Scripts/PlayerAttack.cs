using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public float attackCooldown = 1f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public LayerMask enemyLayers;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
