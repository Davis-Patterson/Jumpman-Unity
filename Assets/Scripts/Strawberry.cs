using System.Collections;
using UnityEngine;

public class Strawberry : MonoBehaviour, IMovable
{
  [SerializeField] private float moveSpeed = 5f;
  [SerializeField] private bool movesRight;

  private Rigidbody2D rb;
  private float moveDirection;
  private bool canMove = false;
  private bool isChangingDirection = false;

  // Raycast parameters
  [SerializeField] private float rayLength = 0.5f;
  [SerializeField] private LayerMask obstacleLayer;

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.gravityScale = 0;
    moveDirection = movesRight ? 1f : -1f;
    StartCoroutine(EnableMovementAfterDelay(1f));
  }

  public void SetInitialDirection(bool movesRight)
  {
    this.movesRight = movesRight;
    moveDirection = movesRight ? 1f : -1f;
  }

  private IEnumerator EnableMovementAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    rb.gravityScale = 1;
    canMove = true;
  }

  private void FixedUpdate()
  {
    if (canMove)
    {
      rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);

      if (!isChangingDirection)
      {
        CheckForObstacles();
      }
    }
  }

  private void CheckForObstacles()
  {
    Vector2 raycastOrigin = new Vector2(transform.position.x + moveDirection * 0.1f, transform.position.y);
    RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, new Vector2(moveDirection, 0), rayLength, obstacleLayer);
    if (hit.collider != null)
    {
      StartCoroutine(ChangeDirection());
    }
  }

  private IEnumerator ChangeDirection()
  {
    isChangingDirection = true;
    yield return new WaitForSeconds(0.1f);
    moveDirection *= -1f;
    isChangingDirection = false;
  }
}
