using System.Collections;
using UnityEngine;

public class PipeExit : MonoBehaviour
{
  [SerializeField] public Transform PointA;
  [SerializeField] public Transform PointB;

  [SerializeField] private AudioSource exitSound;

  private Rigidbody2D playerRigidbody;

  private bool isExiting = false;
  private Coroutine exitCoroutine;

  public void StartExitAnimation(Transform player)
  {
    if (!isExiting)
    {
      exitSound.Play();
      if (exitCoroutine != null) StopCoroutine(exitCoroutine);
      exitCoroutine = StartCoroutine(ExitAnimationCoroutine(player));
    }
  }

  private IEnumerator ExitAnimationCoroutine(Transform player)
  {
    isExiting = true;
    DisablePlayerComponents(player);

    playerRigidbody = player.GetComponent<Rigidbody2D>();

    Vector3 enteredPosition = PointB.position;
    Vector3 targetPosition = AdjustedExitPosition(PointA.position, player);
    Vector3 enteredScale = Vector3.one * 0.5f;

    playerRigidbody.velocity = Vector2.zero;
    playerRigidbody.gravityScale = 0;

    float duration = 1f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
      float t = elapsed / duration;
      player.position = Vector3.Lerp(enteredPosition, targetPosition, t);
      player.localScale = Vector3.Lerp(enteredScale, Vector3.one, t);

      elapsed += Time.deltaTime;
      yield return null;
    }

    yield return new WaitForSeconds(.2f);

    if (!IsOverlapping(player))
    {
      EnablePlayerComponents(player);
    }

    playerRigidbody.gravityScale = 3;
    ResetPlayerMovementState(player);

    isExiting = false;

    playerRigidbody.gravityScale = 3;
    player.GetComponent<PlayerMovement>().enabled = true;
  }

  private void ResetPlayerMovementState(Transform player)
  {
    var playerMovement = player.GetComponent<PlayerMovement>();
    if (playerMovement != null)
    {
      playerMovement.ResetState();
    }
  }

  private Vector3 AdjustedExitPosition(Vector3 exitPosition, Transform player)
  {
    return new Vector3(exitPosition.x, exitPosition.y + 0.5f, exitPosition.z);
  }

  private bool IsOverlapping(Transform player)
  {
    Collider2D playerCollider = player.GetComponent<Collider2D>();
    return Physics2D.OverlapBox(playerCollider.bounds.center, playerCollider.bounds.size, 0) != null;
  }

  private void DisablePlayerComponents(Transform player)
  {
    player.GetComponent<PlayerMovement>().enabled = false;
    player.GetComponent<Collider2D>().enabled = false;
    player.GetComponent<Rigidbody2D>().isKinematic = true;
    // Disable camera follow here if necessary
  }

  private void EnablePlayerComponents(Transform player)
  {
    player.GetComponent<PlayerMovement>().enabled = true;
    player.GetComponent<Collider2D>().enabled = true;
    player.GetComponent<Rigidbody2D>().isKinematic = false;
    // Re-enable camera follow here if necessary
  }
}
