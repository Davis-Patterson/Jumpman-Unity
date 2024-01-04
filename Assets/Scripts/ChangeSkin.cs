using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
  public AnimatorOverrideController playerPowerUp;
  void PowerUp()
  {
    GetComponent<Animator>().runtimeAnimatorController = playerPowerUp as RuntimeAnimatorController;
  }
}
