using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
  [SerializeField] private float bounceForce = 10f;
  private Animator npcAnimator;
  [SerializeField] private float animationDelay = .4f;
  [SerializeField] private AudioSource stompSoundEffect;

  private ScoreManager scoreManager;

  [SerializeField] private int requiredHits = 1;
  private int hitCount = 0;
  private void Start()
  {
    npcAnimator = GetComponentInParent<Animator>();
    scoreManager = FindObjectOfType<ScoreManager>();
    if (stompSoundEffect == null)
    {
      stompSoundEffect = GetComponent<AudioSource>();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
      if (playerRb != null)
      {
        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
      }

      if (npcAnimator != null)
      {
        npcAnimator.SetTrigger("hit");
      }

      if (stompSoundEffect != null)
      {
        stompSoundEffect.Play();
      }

      scoreManager.AddKills(1);

      hitCount++;
      if (hitCount >= requiredHits)
      {
        StartCoroutine(DeactivateAfterDelay(animationDelay));
      }
    }
  }

  private IEnumerator DeactivateAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    transform.parent.gameObject.SetActive(false);
  }
}
