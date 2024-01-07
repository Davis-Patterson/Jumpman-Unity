using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
  [SerializeField] private GameObject strawberryBulletPrefab;
  [SerializeField] private Transform spawnPoint;

  [SerializeField] private AudioSource powerUpSoundEffect;

  private PlayerLife playerLife;
  private Animator anim;
  private RuntimeAnimatorController originalAnimatorController;

  private string previousPowerUpType = "";

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
    DeactivateAllPowerUps();
    if (Scoring.currentPowerUpType != "Pineapple")
    {
      previousPowerUpType = Scoring.currentPowerUpType;
      powerUpSoundEffect.Play();
      anim.SetTrigger("hit");
      playerLife.ModifyHitPoints(1);
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
      }
    }
  }

  public void RePowerUpPrevious()
  {
    if (previousPowerUpType != "")
    {
      PowerUp(previousPowerUpType);
    }
    else
    {
      DeactivateAllPowerUps();
    }
  }

  public void RePowerUp()
  {
    string powerUpType = previousPowerUpType != "" ? previousPowerUpType : Scoring.currentPowerUpType;
    if (powerUpType != "" && anim != null)
    {
      PowerUp(powerUpType);
      playerLife.ModifyHitPoints(1);
    }
    else
    {
      DeactivateAllPowerUps();
    }
  }

  public void DeactivateAllPowerUps()
  {
    if (anim != null)
    {
      hasStrawberryPower = false;
      anim.runtimeAnimatorController = originalAnimatorController;
      Scoring.currentPowerUpType = "";
      previousPowerUpType = "";
    }
  }

  private void StrawberryPowerUp()
  {
    hasStrawberryPower = true;
    playerLife.ModifyHitPoints(1);
  }

  private void KiwiPowerUp()
  {
    return;
  }

  public void ShootStrawberry()
  {
    if (strawberryBulletPrefab && spawnPoint && hasStrawberryPower)
    {
      Instantiate(strawberryBulletPrefab, spawnPoint.position, Quaternion.identity);
    }
  }
}
