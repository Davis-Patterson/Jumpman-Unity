using System.Collections;
using UnityEngine;

public class ChamPatrol : MonoBehaviour
{
  public GameObject pointA;
  public GameObject pointB;
  private Rigidbody2D rb;
  private Animator anim;
  private Transform currentPoint;
  public float speed;
  public float hitSpeed;
  private bool isHit = false;

  private bool isChangingDirection = false;
  public bool towardsB = false;
  private bool isPatrolPaused = false;

  public bool pause = false;
  public float pauseDuration = 2f;

  public float flipOffset = 0.5f;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    currentPoint = towardsB ? pointB.transform : pointA.transform;
    anim.SetBool("isRunning", true);
  }

  void FixedUpdate()
  {
    if (!isPatrolPaused)
    {
      MoveTowardsCurrentPoint();
      CheckIfPointReached();
    }
  }

  public void SetPatrolPaused(bool status)
  {
    isPatrolPaused = status;
    if (isPatrolPaused)
    {
      rb.velocity = Vector2.zero;
      anim.SetBool("isRunning", false);
    }
    else
    {
      anim.SetBool("isRunning", true);
    }
  }

  private void MoveTowardsCurrentPoint()
  {
    if (!isPatrolPaused)
    {
      Vector2 direction = (currentPoint.position - transform.position).normalized;
      float currentSpeed = isHit ? hitSpeed : speed;
      rb.velocity = direction * currentSpeed;
    }
  }

  public void OnHit()
  {
    isHit = true;
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
    anim.SetTrigger("attack");
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
    float directionTowardsPoint = currentPoint.position.x - transform.position.x;

    if ((directionTowardsPoint > 0 && transform.localScale.x < 0) ||
        (directionTowardsPoint < 0 && transform.localScale.x > 0))
    {
      float offsetDirection = transform.localScale.x > 0 ? -1 : 1;

      Vector2 currentVelocity = rb.velocity;
      rb.velocity = Vector2.zero;

      transform.position = new Vector2(transform.position.x + flipOffset * offsetDirection, transform.position.y);

      Vector3 localScale = transform.localScale;
      localScale.x *= -1;
      transform.localScale = localScale;

      rb.velocity = currentVelocity;
    }
  }


  private void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(pointA.transform.position, 0.1f);
    Gizmos.DrawWireSphere(pointB.transform.position, 0.1f);
    Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
  }
}
