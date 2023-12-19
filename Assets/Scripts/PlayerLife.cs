using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
  private Rigidbody2D rb;
  private Animator anim;
  private SpawnMaster gm;

  [SerializeField] private AudioSource hitSoundEffect;
  [SerializeField] private AudioSource deathSoundEffect;
  [SerializeField] private AudioSource spawnSoundEffect;

  [SerializeField] private LivesCounter livesCounter;
  [SerializeField] private int HitPoints = 2;

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
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

  public void TakeDamage()
  {
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
      }
    }
  }

  public void Die()
  {
    deathSoundEffect.Play();
    rb.bodyType = RigidbodyType2D.Static;
    anim.SetTrigger("death");

    LivesCounter.RemoveLife(1);
    if (Scoring.totalLives > 0)
    {
      Invoke("Spawn", 2f);
    }
    HitPoints = 2;
  }

  private void Spawn()
  {
    if (gm != null)
    {
      transform.position = gm.lastCheckpointPos != Vector2.zero ? gm.lastCheckpointPos : gm.transform.position;
      rb.bodyType = RigidbodyType2D.Dynamic;
      spawnSoundEffect.Play();
      anim.SetTrigger("spawn");
    }
  }
}
