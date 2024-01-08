using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
  [SerializeField] private GameObject strawberryBulletPrefab;
  [SerializeField] private Transform spawnPoint;

  [SerializeField] private AudioSource powerUpSoundEffect;

  private PlayerLife playerLife;
  private Animator anim;
  private RuntimeAnimatorController originalAnimatorController;

  private bool isPineapple;
  private bool hasStrawberryPower = false;

  public AnimatorOverrideController playerStrawberry;
  public AnimatorOverrideController playerPineapple;
  public AnimatorOverrideController playerBananaUp;
  public AnimatorOverrideController playerKiwiUp;

  void Start()
  {
    playerLife = GetComponent<PlayerLife>();
    anim = GetComponent<Animator>();
    originalAnimatorController = anim.runtimeAnimatorController;
  }

  void Update()
  {
    if (hasStrawberryPower && Input.GetButtonDown("Fire"))
    {
      ShootStrawberry();
    }
  }

  public void PowerUp(string powerUpType)
  {
    if (powerUpType == Scoring.currentPowerUpType) return;

    if (powerUpType == "None")
    {
      DeactivateAllPowerUps();
      return;
    }

    powerUpSoundEffect.Play();
    anim.SetTrigger("hit_transition");
    if (powerUpType != "Pineapple")
    {
      Scoring.currentPowerUpType = powerUpType;
    }

    switch (powerUpType)
    {
      case "Strawberry":
        playerLife.anim.runtimeAnimatorController = playerStrawberry;
        StrawberryPowerUp();
        break;
      case "Kiwi":
        playerLife.anim.runtimeAnimatorController = playerKiwiUp;
        KiwiPowerUp();
        break;
      case "Banana":
        playerLife.anim.runtimeAnimatorController = playerBananaUp;
        break;
      case "Pineapple":
        playerLife.anim.runtimeAnimatorController = playerPineapple;
        PineapplePowerUp();
        break;
    }
  }

  public void RePowerUp(string powerUpType)
  {
    if (powerUpType == "None")
    {
      DeactivateAllPowerUps();
      return;
    }

    switch (powerUpType)
    {
      case "Strawberry":
        playerLife.anim.runtimeAnimatorController = playerStrawberry;
        StrawberryPowerUp();
        break;
      case "Kiwi":
        playerLife.anim.runtimeAnimatorController = playerKiwiUp;
        KiwiPowerUp();
        break;
      case "Banana":
        playerLife.anim.runtimeAnimatorController = playerBananaUp;
        break;
    }
  }

  public void RevertPineapple(string powerUpType)
  {
    if (powerUpType == "None")
    {
      DeactivateAllPowerUps();
      return;
    }

    anim.SetTrigger("hit_transition");

    switch (powerUpType)
    {
      case "Strawberry":
        playerLife.anim.runtimeAnimatorController = playerStrawberry;
        StrawberryPowerUp();
        break;
      case "Kiwi":
        playerLife.anim.runtimeAnimatorController = playerKiwiUp;
        KiwiPowerUp();
        break;
      case "Banana":
        playerLife.anim.runtimeAnimatorController = playerBananaUp;
        break;
    }
  }

  public void DeactivateAllPowerUps()
  {
    if (anim != null)
    {
      Scoring.currentPowerUpType = "None";
      hasStrawberryPower = false;
      anim.runtimeAnimatorController = originalAnimatorController;
    }
  }


  private void StrawberryPowerUp()
  {
    hasStrawberryPower = true;
  }

  private void KiwiPowerUp()
  {
    return;
  }

  private void PineapplePowerUp()
  {
    StartCoroutine(Pineapple());
  }

  private IEnumerator Pineapple()
  {
    isPineapple = true;
    playerLife.BecomeInvincible();

    yield return new WaitForSeconds(10);

    isPineapple = false;
    playerLife.BecomeVulnerable();
    RevertPineapple(Scoring.currentPowerUpType);
  }

  public void ShootStrawberry()
  {
    if (strawberryBulletPrefab && spawnPoint && hasStrawberryPower)
    {
      Instantiate(strawberryBulletPrefab, spawnPoint.position, Quaternion.identity);
    }
  }
}
