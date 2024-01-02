using System.Collections;
using UnityEngine;

public class BoxHit : MonoBehaviour
{
  public int maxHits = -1;
  public Sprite emptyBlock;

  [SerializeField] private AudioSource hitSound;
  [SerializeField] private AudioSource denySound;
  [SerializeField] private GameObject[] rewards;
  [SerializeField] private float spawnHeight = 2f;
  [SerializeField] private float spawnAnimationDuration = 1f;
  public enum RewardType { Cherry, Melon, Strawberry }
  [SerializeField] private RewardType selectedRewardType;
  [SerializeField] private bool rewardMovesRight = false;

  private Vector3 originalScale;
  private Vector3 originalPosition;

  private void Start()
  {
    originalScale = transform.localScale;
    originalPosition = transform.position;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      if (IsCollisionFromBelow(collision))
      {
        Hit();
      }
    }
  }

  private bool IsCollisionFromBelow(Collision2D collision)
  {
    float boxBottom = transform.position.y - (transform.localScale.y / 2);
    float playerTop = collision.transform.position.y + (collision.transform.localScale.y / 2);

    return playerTop < boxBottom;
  }

  private void Hit()
  {
    if (maxHits != 0)
    {
      hitSound.Play();
      SpawnReward();
      StartCoroutine(HitAnimation(false));
      maxHits--;
    }
    else
    {
      SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
      spriteRenderer.sprite = emptyBlock;
      StartCoroutine(HitAnimation(true));
      denySound.Play();
    }
  }

  private void SpawnReward()
  {
    GameObject rewardPrefab = rewards[(int)selectedRewardType];
    Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
    GameObject spawnedReward = Instantiate(rewardPrefab, spawnPosition, Quaternion.identity);

    IMovable movableReward = spawnedReward.GetComponent<IMovable>();
    movableReward?.SetInitialDirection(rewardMovesRight);

    Vector3 finalScale = Vector3.one;
    if (selectedRewardType == RewardType.Strawberry)
    {
      finalScale *= 1.5f;
    }

    StartCoroutine(SpawnAnimation(spawnedReward, spawnAnimationDuration, finalScale));
  }

  private IEnumerator HitAnimation(bool isBoxEmpty)
  {
    float hitDuration = 0.1f;
    Vector3 increasedScale = isBoxEmpty ? originalScale * 1.025f : originalScale * 1.05f;
    Vector3 nudgedPosition = isBoxEmpty ? originalPosition + new Vector3(0, 0.125f, 0) : originalPosition + new Vector3(0, 0.25f, 0);

    transform.localScale = increasedScale;
    transform.position = nudgedPosition;

    yield return new WaitForSeconds(hitDuration / 2);

    transform.localScale = originalScale;
    transform.position = originalPosition;

    yield return new WaitForSeconds(hitDuration / 2);
  }

  private IEnumerator SpawnAnimation(GameObject reward, float duration, Vector3 finalScale)
  {
    float elapsedTime = 0;
    Vector3 startScale = new Vector3(0.7f, 0.7f, 0.7f);
    Vector3 startPosition = reward.transform.position;
    Vector3 endPosition = startPosition + new Vector3(0, spawnHeight, 0);

    while (elapsedTime < duration)
    {
      float t = elapsedTime / duration;
      t = t * t * t * (t * (6f * t - 15f) + 10f);

      reward.transform.position = Vector3.Lerp(startPosition, endPosition, t);
      reward.transform.localScale = Vector3.Lerp(startScale, finalScale, t);

      elapsedTime += Time.deltaTime;
      yield return null;
    }

    reward.transform.position = endPosition;
    reward.transform.localScale = finalScale;
  }
}