using UnityEngine;
using System.Collections;

public class ItemCollector : MonoBehaviour
{
  [SerializeField] private AudioSource collectionSoundEffect;
  [SerializeField] private AudioSource lifeSoundEffect;

  private ScoreManager scoreManager;
  private PlayerPowerUp powerUpScript;

  private void Start()
  {
    scoreManager = FindObjectOfType<ScoreManager>();
    powerUpScript = GetComponent<PlayerPowerUp>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Cherry") || collision.gameObject.CompareTag("Melon") ||
        collision.gameObject.CompareTag("Strawberry") || collision.gameObject.CompareTag("Pineapple") ||
        collision.gameObject.CompareTag("Kiwi") || collision.gameObject.CompareTag("Banana"))
    {
      TriggerCollectionAnimation(collision.gameObject);
      collectionSoundEffect.Play();

      switch (collision.gameObject.tag)
      {
        case "Cherry":
          scoreManager.AddCherries(1);
          break;
        case "Melon":
          StartCoroutine(PlayLifeSoundAfterDelay(0.12f));
          scoreManager.AddMelons(1);
          LivesCounter.AddLife(1);
          break;
        case "Strawberry":
          scoreManager.AddStrawberries(1);
          powerUpScript.PowerUp("Strawberry");
          break;
        case "Pineapple":
          scoreManager.AddPineapples(1);
          powerUpScript.PowerUp("Pineapple");
          break;
        case "Kiwi":
          scoreManager.AddKiwis(1);
          powerUpScript.PowerUp("Kiwi");
          break;
        case "Banana":
          scoreManager.AddBananas(1);
          powerUpScript.PowerUp("Banana");
          break;
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
