using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
  private PlayerPowerUp playerPowerUp;
  private Rigidbody2D rb;
  public Animator anim;
  private SpawnMaster gm;
  private BoxCollider2D coll;
  private Vector2 originalColliderSize;

  [SerializeField] private AudioSource hitSoundEffect;
  [SerializeField] private AudioSource deathSoundEffect;
  [SerializeField] private AudioSource spawnSoundEffect;

  [SerializeField] private LivesCounter livesCounter;

  private Vector3 originalScale;

  private bool isRespawning = false;
  private bool isInvincible = false;


  private void Awake()
  {
    anim = GetComponent<Animator>();
    coll = GetComponent<BoxCollider2D>();
    originalColliderSize = coll.size;
    originalScale = transform.localScale;
    playerPowerUp = GetComponent<PlayerPowerUp>();
  }

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    gm = GameObject.FindGameObjectWithTag("SpawnMaster").GetComponent<SpawnMaster>();

    if (gm != null)
    {
      Vector2 startPosition = gm.lastCheckpointPos != Vector2.zero ? gm.lastCheckpointPos : gm.transform.position;
      transform.position = startPosition;
    }

    playerPowerUp.RePowerUp(Scoring.currentPowerUpType);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Trap"))
    {
      TakeDamage();
    }
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.CompareTag("Trap") || collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Trunk"))
    {
      TakeDamage();
    }
  }

  public void TakeDamage()
  {
    if (isRespawning || isInvincible) return;

    if (Scoring.currentPowerUpType != "None")
    {
      playerPowerUp.DeactivateAllPowerUps();
      anim.SetTrigger("hit");
      if (hitSoundEffect != null)
      {
        hitSoundEffect.Play();
        StartCoroutine(ScaleOverTime(0.5f, 1f, originalColliderSize));
      }
    }
    else
    {
      Die();
    }
  }

  public void Die()
  {
    if (isRespawning || isInvincible) return;

    isRespawning = true;
    Scoring.currentPowerUpType = "None";
    deathSoundEffect.Play();
    rb.bodyType = RigidbodyType2D.Static;
    anim.SetTrigger("death");

    StartCoroutine(DisableCollidersAfterAnimation(0.25f));

    LivesCounter.RemoveLife(1);

    if (Scoring.totalLives > 0)
    {
      Invoke("Spawn", 1.75f);
    }
  }

  private void Spawn()
  {
    isRespawning = false;

    foreach (var collider in GetComponents<Collider2D>())
    {
      collider.enabled = true;
    }

    if (gm != null)
    {
      playerPowerUp.RePowerUp(Scoring.currentPowerUpType);
      transform.position = gm.lastCheckpointPos != Vector2.zero ? gm.lastCheckpointPos : gm.transform.position;
      rb.bodyType = RigidbodyType2D.Dynamic;
      spawnSoundEffect.Play();
      anim.SetTrigger("spawn");
    }
  }

  public void BecomeInvincible()
  {
    isInvincible = true;
  }

  public void BecomeVulnerable()
  {
    isInvincible = false;
  }

  private IEnumerator ScaleOverTime(float duration, float targetScaleFactor, Vector2 originalColliderSize)
  {
    float currentTime = 0;
    Vector3 targetScaleVector = originalScale * targetScaleFactor;

    float deltaYScale = targetScaleVector.y - transform.localScale.y;

    Vector2 originalOffset = coll.offset;

    float fineTuningFactor = 1.5f;
    Vector2 targetOffset = new Vector2(originalOffset.x, originalOffset.y + (deltaYScale / 2) * fineTuningFactor);

    while (currentTime < duration)
    {
      currentTime += Time.deltaTime;
      float t = currentTime / duration;

      Vector3 newScale = Vector3.Lerp(transform.localScale, targetScaleVector, t);
      transform.localScale = newScale;

      coll.size = Vector2.Lerp(coll.size, originalColliderSize * targetScaleFactor, t);

      coll.offset = Vector2.Lerp(coll.offset, targetOffset, t);

      yield return null;
    }

    transform.localScale = targetScaleVector;
    coll.size = originalColliderSize * targetScaleFactor;
    coll.offset = targetOffset;
  }

  private IEnumerator DisableCollidersAfterAnimation(float delay)
  {
    yield return new WaitForSeconds(delay);

    foreach (var collider in GetComponents<Collider2D>())
    {
      collider.enabled = false;
    }
  }
}