using System.Collections;
using UnityEngine;

public class PipeEnter : MonoBehaviour
{
  [SerializeField] private Transform PointA;
  [SerializeField] private Transform PointB;
  [SerializeField] private Transform ExitPipe;
  [SerializeField] private bool HorizontalPipe = false;

  private Transform playerTransform;
  [SerializeField] private Collider2D tilemapCollider;

  [SerializeField] private AudioSource enterSound;

  private bool isEntering = false;
  private Coroutine enterCoroutine;

  private void Awake()
  {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (!isEntering && ExitPipe != null && other.CompareTag("Player"))
    {
      bool enterPipe = false;

      if (HorizontalPipe)
      {
        float horizontalInput = Input.GetAxis("Horizontal");
        enterPipe = Mathf.Abs(horizontalInput) > 0;
      }
      else
      {
        float verticalInput = Input.GetAxis("Vertical");
        enterPipe = verticalInput < 0;
      }

      if (enterPipe)
      {
        enterSound.Play();
        playerTransform.position = ExitPipe.GetComponent<PipeExit>().PointB.position;
        if (enterCoroutine != null) StopCoroutine(enterCoroutine);
        enterCoroutine = StartCoroutine(Enter(other.transform));
      }
    }
  }


  private void Start()
  {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private IEnumerator Enter(Transform player)
  {
    isEntering = true;
    DisablePlayerComponents(player);
    ResetPlayerMovementState(player);

    tilemapCollider.enabled = false;

    Vector3 enteredPosition = PointA.position;
    Vector3 targetPosition = PointB.position;
    Vector3 enteredScale = Vector3.one * 0.5f;

    float duration = 1f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
      float t = elapsed / duration;
      player.position = Vector3.Lerp(enteredPosition, targetPosition, t);
      player.localScale = Vector3.Lerp(Vector3.one, enteredScale, t);

      elapsed += Time.deltaTime;
      yield return null;
    }

    ExitPipe.GetComponent<PipeExit>().StartExitAnimation(player);

    yield return new WaitForSeconds(1f);

    isEntering = false;

    tilemapCollider.enabled = true;

    player.GetComponent<PlayerMovement>().enabled = true;
  }

  private void DisablePlayerComponents(Transform player)
  {
    player.GetComponent<PlayerMovement>().enabled = false;
    player.GetComponent<Collider2D>().enabled = false;
    player.GetComponent<Rigidbody2D>().isKinematic = true;
  }

  private void EnablePlayerComponents(Transform player)
  {
    player.GetComponent<PlayerMovement>().enabled = true;
    player.GetComponent<Collider2D>().enabled = true;
    player.GetComponent<Rigidbody2D>().isKinematic = false;
  }

  private void ResetPlayerMovementState(Transform player)
  {
    var playerMovement = player.GetComponent<PlayerMovement>();
    if (playerMovement != null)
    {
      playerMovement.ResetState();
    }
  }
}
