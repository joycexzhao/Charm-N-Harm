using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{

    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    //public Animator animator2;
    public float delay = 0.3f;
    public bool attackBlocked;

    public bool IsAttacking { get; private set; }

    public Transform circleOrigin;
    public float radius;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    public GameObject hitSound;

    private void Update(){
        if (IsAttacking)
        {
            return;
        }
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
    }

    public void Attack()
    {
        if (attackBlocked)
        {
            return;
        }
        animator.SetTrigger("Attack");
        //animator2.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        hitSound.GetComponent<AudioSource>().Play();
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            Debug.Log(collider.name);
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(1, transform.parent.gameObject);
            }

            if (collider.GetComponent<Rock>())
            {
                Destroy(collider.gameObject);
            }
        }
    }

}
