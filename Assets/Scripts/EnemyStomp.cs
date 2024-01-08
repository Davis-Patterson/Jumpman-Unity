using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
  [SerializeField] private float bounceForce = 14f;
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

      hitCount++;

      if (stompSoundEffect != null && hitCount <= requiredHits)
      {
        TriggerStompSound();
      }

      if (hitCount < requiredHits)
      {
        scoreManager.AddKills(1);
      }
      else if (hitCount >= requiredHits)
      {
        scoreManager.AddKills(1);
        StartCoroutine(DeactivateAfterDelay(animationDelay));
      }
    }
  }

  public void TriggerStompSound()
  {
    stompSoundEffect.Play();
  }

  private IEnumerator DeactivateAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    transform.parent.gameObject.SetActive(false);
  }
}
