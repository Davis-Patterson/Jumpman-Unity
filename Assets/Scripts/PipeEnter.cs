using System.Collections;
using UnityEngine;

public class PipeEnter : MonoBehaviour
{
  [SerializeField] private Transform PointA;
  [SerializeField] private Transform PointB;
  [SerializeField] private Transform ExitPipe;
  [SerializeField] private string EnterDirection = "Down";
  [SerializeField] private AudioSource enterSound;

  private Transform playerTransform;
  private bool isEntering = false;

  private void Start()
  {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (!isEntering && ExitPipe != null && other.CompareTag("Player") && CheckEnterPipeCondition())
    {
      enterSound?.Play();
      StartCoroutine(Enter(other.transform));
    }
  }

  private bool CheckEnterPipeCondition()
  {
    switch (EnterDirection.ToLower())
    {
      case "down": return Input.GetAxis("Vertical") < 0;
      case "up": return Input.GetAxis("Vertical") > 0;
      case "left": return Input.GetAxis("Horizontal") < 0;
      case "right": return Input.GetAxis("Horizontal") > 0;
      default:
        Debug.LogError("Invalid Enter Direction set in the Inspector!");
        return false;
    }
  }

  private IEnumerator Enter(Transform player)
  {
    isEntering = true;
    DisablePlayerComponents(player);

    Vector3 enteredPosition = PointA.position;
    Vector3 targetPosition = PointB.position;
    float duration = 1f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
      float t = elapsed / duration;
      player.position = Vector3.Lerp(enteredPosition, targetPosition, t);
      player.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.5f, t);
      elapsed += Time.deltaTime;
      yield return null;
    }

    ExitPipe.GetComponent<PipeExit>().StartExitAnimation(player);
    yield return new WaitForSeconds(1f);

    isEntering = false;
    EnablePlayerComponents(player);
  }

  private void DisablePlayerComponents(Transform player)
  {
    var playerMovement = player.GetComponent<PlayerMovement>();
    var playerCollider = player.GetComponent<Collider2D>();
    var playerRigidbody = player.GetComponent<Rigidbody2D>();

    playerMovement.enabled = false;
    playerCollider.enabled = false;
  }

  private void EnablePlayerComponents(Transform player)
  {
    var playerMovement = player.GetComponent<PlayerMovement>();
    var playerCollider = player.GetComponent<Collider2D>();
    var playerRigidbody = player.GetComponent<Rigidbody2D>();

    playerMovement.enabled = true;
    playerCollider.enabled = true;
    if (playerRigidbody != null)
    {
      playerRigidbody.isKinematic = false;
    }
  }
}
