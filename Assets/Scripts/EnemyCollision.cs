using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
  private EnemyStomp enemyStomp;
  private Animator npcAnimator;
  private float animationDelay = .4f;

  private void Start()
  {
    npcAnimator = GetComponentInParent<Animator>();
    enemyStomp = GetComponentInParent<EnemyStomp>();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      if (collision.otherCollider.gameObject.tag != "WeakPoint")
      {
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
        {
          playerLife.TakeDamage();
        }
      }
    }
    if (collision.gameObject.tag == "Invincible")
    {
      HandleEnemyHit();
    }
    if (collision.gameObject.tag == "StrawberryBullet")
    {
      HandleEnemyHit();
    }
  }

  private void HandleEnemyHit()
  {
    Debug.Log("Enemy Hit");
    if (npcAnimator != null)
    {
      npcAnimator.SetTrigger("hit");
    }

    if (enemyStomp != null)
    {
      enemyStomp.TriggerStompSound();
    }

    StartCoroutine(DeactivateAfterDelay(animationDelay));
  }

  private IEnumerator DeactivateAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    transform.parent.gameObject.SetActive(false);
  }
}
