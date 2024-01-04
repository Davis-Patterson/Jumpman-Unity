using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailStomp : MonoBehaviour
{
  [SerializeField] private float bounceForce = 12f;
  private Animator npcAnimator;
  [SerializeField] private float animationDelay = .4f;
  [SerializeField] private AudioSource stompSoundEffect;

  private ScoreManager scoreManager;

  [SerializeField] private int requiredHits = 2;
  private int hitCount = 0;
  private bool isVulnerable = false;

  private EnemyPatrol enemyPatrol;

  private void Start()
  {
    npcAnimator = GetComponentInParent<Animator>();
    scoreManager = FindObjectOfType<ScoreManager>();
    enemyPatrol = GetComponentInParent<EnemyPatrol>();
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

      HandleStomp();
    }
  }

  private void HandleStomp()
  {
    if (hitCount < requiredHits - 1)
    {
      EnterShell();
    }
    else if (hitCount == requiredHits - 1 && isVulnerable)
    {
      FinalHit();
    }
  }

  private void EnterShell()
  {
    if (npcAnimator != null)
    {
      npcAnimator.SetTrigger("hit");
    }

    if (stompSoundEffect != null)
    {
      stompSoundEffect.Play();
    }

    hitCount++;
    isVulnerable = true;
    npcAnimator.SetTrigger("EnterShell");

    if (enemyPatrol != null)
    {
      enemyPatrol.SetPatrolPaused(true);
      enemyPatrol.OnHit();
    }

    StartCoroutine(VulnerabilityWindow());
  }

  private void FinalHit()
  {
    if (npcAnimator != null)
    {
      npcAnimator.SetTrigger("hit_shell");
    }

    if (stompSoundEffect != null)
    {
      stompSoundEffect.Play();
    }

    scoreManager.AddKills(1);
    hitCount = 0;
    isVulnerable = false;
    StartCoroutine(DeactivateAfterDelay(animationDelay));
  }

  private IEnumerator VulnerabilityWindow()
  {
    yield return new WaitForSeconds(4f);

    if (isVulnerable)
    {
      isVulnerable = false;
      npcAnimator.SetTrigger("ExitShell");

      hitCount = 0;

      if (enemyPatrol != null)
      {
        enemyPatrol.SetPatrolPaused(false);
      }
    }
  }

  private IEnumerator DeactivateAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    transform.parent.gameObject.SetActive(false);
  }
}
