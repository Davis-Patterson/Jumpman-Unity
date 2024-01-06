using UnityEngine;
using System.Collections;

public class ItemCollector : MonoBehaviour
{
  [SerializeField] private AudioSource collectionSoundEffect;
  [SerializeField] private AudioSource lifeSoundEffect;

  private ScoreManager scoreManager;

  private void Start()
  {
    scoreManager = FindObjectOfType<ScoreManager>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Cherry") || collision.gameObject.CompareTag("Melon") || collision.gameObject.CompareTag("Strawberry") || collision.gameObject.CompareTag("Pineapple"))
    {
      TriggerCollectionAnimation(collision.gameObject);
      collectionSoundEffect.Play();

      if (collision.gameObject.CompareTag("Cherry"))
      {
        scoreManager.AddCherries(1);
      }
      else if (collision.gameObject.CompareTag("Melon"))
      {
        StartCoroutine(PlayLifeSoundAfterDelay(0.12f));
        scoreManager.AddMelons(1);
        LivesCounter.AddLife(1);
      }
      else if (collision.gameObject.CompareTag("Strawberry"))
      {
        TriggerCollectionAnimation(collision.gameObject);
        collectionSoundEffect.Play();
        scoreManager.AddStrawberries(1);

        PlayerLife playerLife = this.GetComponent<PlayerLife>();
        if (playerLife != null)
        {
          playerLife.PowerUp();
        }
      }
      else if (collision.gameObject.CompareTag("Pineapple"))
      {
        TriggerCollectionAnimation(collision.gameObject);
        collectionSoundEffect.Play();
        scoreManager.AddPineapples(1);

        PlayerLife playerLife = this.GetComponent<PlayerLife>();
        if (playerLife != null)
        {
          playerLife.PowerUp();
        }
      }
      StartCoroutine(DeactivateAfterDelay(collision.gameObject, 1f));
    }
  }

  private IEnumerator DeactivateAfterDelay(GameObject gameObject, float delay)
  {
    yield return new WaitForSeconds(delay);
    gameObject.SetActive(false);
  }

  private void TriggerCollectionAnimation(GameObject fruit)
  {
    Animator fruitAnimator = fruit.GetComponent<Animator>();
    if (fruitAnimator != null)
    {
      fruitAnimator.SetTrigger("collected");
    }
  }


  private IEnumerator PlayLifeSoundAfterDelay(float delay)
  {
    yield return new WaitForSeconds(delay);
    lifeSoundEffect.Play();
  }
}
