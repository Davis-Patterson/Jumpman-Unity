using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
    public bool pause = false;
    public float pauseDuration = 2f;
    private bool isChangingDirection = false;
    public bool towardsB = false;

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    currentPoint = towardsB ? pointB.transform : pointA.transform; // Set initial target based on towardsB
    anim.SetBool("isRunning", true);
}

    void FixedUpdate()
    {
        MoveTowardsCurrentPoint();
        CheckIfPointReached();
    }

    private void MoveTowardsCurrentPoint()
    {
        if (!isChangingDirection)
        {
            Vector2 direction = (currentPoint.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void CheckIfPointReached()
    {
        if (!isChangingDirection && Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            isChangingDirection = true;
            if (pause)
            {
                StartCoroutine(PauseBeforeFlip());
            }
            else
            {
                flip();
                ChangeDirection();
            }
        }
    }

    IEnumerator PauseBeforeFlip()
    {
        rb.velocity = Vector2.zero; 
        anim.SetBool("isRunning", false);
        yield return new WaitForSeconds(pauseDuration);
        flip();
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
        anim.SetBool("isRunning", true);
        isChangingDirection = false;
    }

private void flip()
{
    // Determine the direction towards the current target point
    float directionTowardsPoint = currentPoint.position.x - transform.position.x;

    // Flip if the direction towards the point is opposite to the current facing
    if ((directionTowardsPoint > 0 && transform.localScale.x < 0) || 
        (directionTowardsPoint < 0 && transform.localScale.x > 0))
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.1f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.1f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
