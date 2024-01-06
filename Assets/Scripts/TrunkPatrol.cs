using System.Collections;
using UnityEngine;

public class TrunkPatrol : MonoBehaviour
{
  public GameObject pointA;
  public GameObject pointB;
  private Rigidbody2D rb;
  private Animator anim;
  private Transform currentPoint;
  public float speed;
  private bool isChangingDirection = false;
  public bool towardsB = false;
  private bool isPatrolPaused = false;

  public bool pause = false;
  public float pauseDuration = 2f;

  private TrunkShoot trunkShoot;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    trunkShoot = GetComponent<TrunkShoot>();
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

  private void MoveTowardsCurrentPoint()
  {
    if (!isPatrolPaused)
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
        StartCoroutine(PauseAndShoot());
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

    GetComponent<TrunkShoot>().Shoot();

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
      Vector3 localScale = transform.localScale;
      localScale.x *= -1;
      transform.localScale = localScale;
      rb.velocity = new Vector2(0, rb.velocity.y);
    }
  }

  private IEnumerator PauseAndShoot()
  {
    SetPatrolPaused(true);
    anim.SetBool("isRunning", false);


    yield return trunkShoot.Shoot();

    flip();
    ChangeDirection();

    SetPatrolPaused(false);
    anim.SetBool("isRunning", true);
  }

  public void SetPatrolPaused(bool state)
  {
    isPatrolPaused = state;
    rb.velocity = Vector2.zero;
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(pointA.transform.position, 0.1f);
    Gizmos.DrawWireSphere(pointB.transform.position, 0.1f);
    Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
  }
}
