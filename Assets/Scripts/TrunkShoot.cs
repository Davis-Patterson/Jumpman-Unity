using System.Collections;
using UnityEngine;

public class TrunkShoot : MonoBehaviour
{
  public GameObject bulletPrefab;
  public Transform spawnPos;
  public float bulletDelay = 0.5f;
  public int numberOfShots = 3;
  public int numberOfSequences = 3;
  public float fireRate = 0.5f;
  public float initialDelay = 0.5f;
  public float sequenceDelay = 1.0f;
  [SerializeField] private AudioSource shotSound;

  private TrunkPatrol trunkPatrol;
  private Animator anim;

  void Start()
  {
    trunkPatrol = GetComponent<TrunkPatrol>();
    anim = GetComponent<Animator>();
  }

  public void StartShooting()
  {
    StartCoroutine(Shoot());
  }

  public IEnumerator Shoot()
  {
    trunkPatrol.SetPatrolPaused(true);

    yield return new WaitForSeconds(initialDelay);

    for (int seq = 0; seq < numberOfSequences; seq++)
    {
      anim.SetTrigger("attack");
      yield return StartCoroutine(ExecuteShootingSequence());

      if (seq < numberOfSequences - 1)
        yield return new WaitForSeconds(sequenceDelay);
    }

    trunkPatrol.SetPatrolPaused(false);
  }


  private IEnumerator ExecuteShootingSequence()
  {
    for (int i = 0; i < numberOfShots; i++)
    {
      GameObject bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
      float direction = Mathf.Sign(transform.localScale.x) * -1;
      bullet.GetComponent<Bullet>().Initialize(direction);

      shotSound.Play();

      if (i < numberOfShots - 1)
        yield return new WaitForSeconds(fireRate);
    }
  }
}
