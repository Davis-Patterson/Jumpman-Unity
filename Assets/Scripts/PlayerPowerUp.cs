using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
  [SerializeField] private GameObject strawberryBulletPrefab;
  [SerializeField] private Transform spawnPoint;

  [SerializeField] private AudioSource powerUpSoundEffect;
  [SerializeField] private AudioSource strawberryShootSoundEffect;

  private PlayerLife playerLife;
  private PlayerMovement playerMovement;
  private Animator anim;
  private RuntimeAnimatorController originalAnimatorController;

  private bool isPineapple;
  private bool hasStrawberryPower = false;
  private float strawberryCooldown = .5f;
  private float timeSinceLastShot = 0f;

  public AnimatorOverrideController playerStrawberry;
  public AnimatorOverrideController playerPineapple;
  public AnimatorOverrideController playerBananaUp;
  public AnimatorOverrideController playerKiwiUp;

  void Start()
  {
    playerLife = GetComponent<PlayerLife>();
    playerMovement = GetComponent<PlayerMovement>();
    anim = GetComponent<Animator>();
    originalAnimatorController = anim.runtimeAnimatorController;
  }

  void Update()
  {
    if (hasStrawberryPower && Input.GetButtonDown("Fire1") && timeSinceLastShot >= strawberryCooldown)
    {
      ShootStrawberry();
      timeSinceLastShot = 0f;
    }


    timeSinceLastShot += Time.deltaTime;
  }

  public void PowerUp(string powerUpType)
  {
    if (powerUpType == Scoring.currentPowerUpType) return;

    if (powerUpType == "None")
    {
      DeactivateAllPowerUps();
      return;
    }

    if (isPineapple)
    {
      if (powerUpType != "Pineapple")
      {
        Scoring.currentPowerUpType = powerUpType;
        return;
      }
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
    if (isPineapple)
    {
      playerLife.BecomeInvincible();
    }

    yield return new WaitForSeconds(10);

    isPineapple = false;
    playerLife.BecomeVulnerable();
    RevertPineapple(Scoring.currentPowerUpType);
  }

  public void ShootStrawberry()
  {
    if (strawberryBulletPrefab && spawnPoint && hasStrawberryPower && timeSinceLastShot >= strawberryCooldown)
    {
      anim.SetTrigger("hit_transition");
      GameObject bullet = Instantiate(strawberryBulletPrefab, spawnPoint.position, Quaternion.identity);

      float direction = playerMovement.IsFacingRight ? 1f : -1f;
      bullet.GetComponent<StrawberryBullet>().Initialize(direction);
      strawberryShootSoundEffect.Play();

      timeSinceLastShot = 0f;
    }
  }

}
