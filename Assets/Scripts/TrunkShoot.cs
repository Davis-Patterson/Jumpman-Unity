using System.Collections;
using UnityEngine;

public class TrunkShoot : MonoBehaviour
{
  public GameObject bulletPrefab;
  public Transform spawnPos;
  public float bulletDelay = 0.5f;

  public void Shoot()
  {
    StartCoroutine(DelayedShoot());
  }

  private IEnumerator DelayedShoot()
  {
    yield return new WaitForSeconds(bulletDelay);

    GameObject bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
    float direction = Mathf.Sign(transform.localScale.x) * -1;
    bullet.GetComponent<Bullet>().Initialize(direction);
  }
}
