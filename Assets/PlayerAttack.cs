using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask enemyLayers;
    public Camera cam;
    public Transform attackPoint;
    Vector2 mousePos;
    public float attackTime = .3f;
    private float attackCounter = .7f;
    public float throwTime = .5f;
    public float attackRange = 0.8f;
    public int attackDamage = 50;
    private bool isAttackingSword;
    public bool isThrowing;

    // Update is called once per frame
    void Update()
    {
        if (isAttackingSword) {
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0) {
                animator.SetBool("isAttackingSword", false);
                isAttackingSword = false;
            }
        } else if (isThrowing) {
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0) {
                animator.SetBool("isThrowing", false);
                isThrowing = false;
            }
        } else if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = mousePos - rb.position;
            animator.SetFloat("mouseX", lookDir.x);
            animator.SetFloat("mouseY", lookDir.y);
            animator.SetFloat("lastMoveX", lookDir.x);
            animator.SetFloat("lastMoveY", lookDir.y);
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Attack();
            } else if (Input.GetKeyDown(KeyCode.Mouse1)) {
                attackCounter = throwTime;
                animator.SetBool("isThrowing", true);
                isThrowing = true;
            }
        }
    }

    void Attack()
    {
        attackCounter = attackTime;
        animator.SetBool("isAttackingSword", true);
        isAttackingSword = true;
    }

    public void AnimationAttack(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
