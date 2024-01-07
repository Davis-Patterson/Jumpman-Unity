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
  [SerializeField] private int HitPoints = 1;

  private Vector3 originalScale;

  private bool isRespawning = false;


  private void Awake()
  {
    anim = GetComponent<Animator>();
    coll = GetComponent<BoxCollider2D>();
    originalColliderSize = coll.size;
    originalScale = transform.localScale;
    playerPowerUp = GetComponent<PlayerPowerUp>();

    if (Scoring.currentPowerUpType != "")
    {
      playerPowerUp.RePowerUp();
    }
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
    if (collider.gameObject.CompareTag("Trap") || collider.gameObject.CompareTag("Enemy"))
    {
      TakeDamage();
    }
  }

  public void TakeDamage()
  {
    if (isRespawning) return;

    HitPoints--;

    if (HitPoints <= 0)
    {
      Die();
    }
    else
    {
      anim.SetTrigger("hit");
      if (hitSoundEffect != null)
      {
        hitSoundEffect.Play();
        StartCoroutine(ScaleOverTime(0.5f, 1f, originalColliderSize));
        HitPoints = 1;
        playerPowerUp.DeactivateAllPowerUps();
      }
    }
  }

  public void Die()
  {
    if (isRespawning) return;

    isRespawning = true;
    Scoring.currentPowerUpType = "";
    deathSoundEffect.Play();
    rb.bodyType = RigidbodyType2D.Static;
    anim.SetTrigger("death");

    LivesCounter.RemoveLife(1);
    if (Scoring.totalLives > 0)
    {
      Invoke("Spawn", 2f);
    }
    HitPoints = 1;
  }

  private void Spawn()
  {
    isRespawning = false;
    if (gm != null)
    {
      if (Scoring.currentPowerUpType != "")
      {
        playerPowerUp.RePowerUp();
      }
      transform.position = gm.lastCheckpointPos != Vector2.zero ? gm.lastCheckpointPos : gm.transform.position;
      rb.bodyType = RigidbodyType2D.Dynamic;
      spawnSoundEffect.Play();
      anim.SetTrigger("spawn");
    }
  }

  public void ModifyHitPoints(int amount)
  {
    HitPoints += amount;
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
}