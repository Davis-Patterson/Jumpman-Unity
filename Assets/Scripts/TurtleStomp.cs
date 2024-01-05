using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleStomp : MonoBehaviour
{
  [SerializeField] private float bounceForce = 14f;
  private Animator turtleAnimator;
  [SerializeField] private float animationDelay = .4f;
  [SerializeField] private AudioSource stompSoundEffect;
  private ScoreManager scoreManager;
  private TurtleSpikes parentTurtleSpikes;

  private void Start()
  {
    turtleAnimator = GetComponentInParent<Animator>();
    scoreManager = FindObjectOfType<ScoreManager>();
    parentTurtleSpikes = GetComponentInParent<TurtleSpikes>();
    if (stompSoundEffect == null)
    {
      stompSoundEffect = GetComponent<AudioSource>();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player" && collision.contacts[0].normal.y > 0.5 && !parentTurtleSpikes.IsDangerous())
    {
      Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
      if (playerRb != null)
      {
        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
      }

      if (turtleAnimator != null)
      {
        turtleAnimator.SetTrigger("hit");
      }

      if (stompSoundEffect != null)
      {
        stompSoundEffect.Play();
      }

      scoreManager.AddKills(1);
      StartCoroutine(DeactivateAfterDelay(animationDelay));
    }
  }

  private IEnumerator DeactivateAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    transform.parent.gameObject.SetActive(false);
  }
}
