using System.Collections;
using UnityEngine;

public class BulletStomp : MonoBehaviour
{
  [SerializeField] private float bounceForce = 14f;
  [SerializeField] private AudioSource stompSoundEffect;
  private ScoreManager scoreManager;

  private void Start()
  {
    if (stompSoundEffect == null)
    {
      stompSoundEffect = GetComponent<AudioSource>();
    }

    scoreManager = FindObjectOfType<ScoreManager>();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
      if (playerRb != null)
      {
        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
      }

      if (scoreManager != null)
      {
        scoreManager.AddScore(100);
      }


      if (stompSoundEffect != null)
      {
        stompSoundEffect.Play();
        StartCoroutine(DeactivateAfterSound(stompSoundEffect.clip.length));
      }
      else
      {
        Debug.LogWarning("Stomp sound effect is not assigned or AudioSource is missing.");
        gameObject.SetActive(false);
      }
    }
  }

  private IEnumerator DeactivateAfterSound(float delay)
  {
    yield return new WaitForSeconds(delay);
    gameObject.SetActive(false);
  }
}
