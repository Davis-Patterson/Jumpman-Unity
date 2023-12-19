using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleSpikes : MonoBehaviour
{
  public float transitionTime = 2f;
  private float timer;
  private bool isSpikes = false;
  private bool isDangerous = false;
  private Animator animator;

  [SerializeField] private float animationDelay = .4f;

  [SerializeField] private float bounceForce = 10f;
  [SerializeField] private AudioSource stompSoundEffect;
  private ScoreManager scoreManager;

  void Start()
  {
    animator = GetComponent<Animator>();
    timer = transitionTime;
    scoreManager = FindObjectOfType<ScoreManager>();
    if (stompSoundEffect == null)
    {
      stompSoundEffect = GetComponent<AudioSource>();
    }
  }

  void Update()
  {
    timer -= Time.deltaTime;

    if (timer <= 0)
    {
      isSpikes = !isSpikes;
      animator.SetBool("isSpikes", isSpikes);
      timer = transitionTime;
      StartCoroutine(UpdateDangerState());
    }
  }

  private IEnumerator UpdateDangerState()
  {
    yield return new WaitForSeconds(0.5f);
    isDangerous = isSpikes;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
      PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();

      if (playerRb != null)
      {
        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
      }

      if (isDangerous)
      {
        if (playerLife != null)
        {
          playerLife.TakeDamage();
        }
      }
      else
      {
        if (animator != null)
        {
          animator.SetTrigger("hit");
        }

        if (stompSoundEffect != null)
        {
          stompSoundEffect.Play();
        }
        scoreManager.AddKills(1);
        StartCoroutine(DeactivateAfterDelay(animationDelay));
      }
    }
  }

  private IEnumerator DeactivateAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    gameObject.SetActive(false);
  }
}
