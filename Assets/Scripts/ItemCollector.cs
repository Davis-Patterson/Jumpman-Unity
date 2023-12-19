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
        if (collision.gameObject.CompareTag("Cherry"))
        {
            collectionSoundEffect.Play();
            collision.gameObject.SetActive(false);
            scoreManager.AddCherries(1); 
        }
        else if (collision.gameObject.CompareTag("Melon"))
        {
            collectionSoundEffect.Play();
            StartCoroutine(PlayLifeSoundAfterDelay(0.12f));
            collision.gameObject.SetActive(false);
            scoreManager.AddMelons(1); 
            LivesCounter.AddLife(1);
        }
    }

    private IEnumerator PlayLifeSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lifeSoundEffect.Play();
    }
}
