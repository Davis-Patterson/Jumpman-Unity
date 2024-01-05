using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scoring : MonoBehaviour
{
  public static int totalCherries;
  public static int totalMelons;
  public static int totalStrawberries;
  public static int totalScore;
  public static int totalLevel;
  public static int totalLives = 5;
  public static int totalKills;
  public static bool isPlayerPoweredUp = false;

  static Scoring()
  {
    ResetGame();
  }

  public static void ResetGame()
  {
    totalCherries = 0;
    totalMelons = 0;
    totalStrawberries = 0;
    totalScore = 0;
    totalLevel = 0;
    totalLives = 5;
    totalKills = 0;
    isPlayerPoweredUp = false;
  }
}
