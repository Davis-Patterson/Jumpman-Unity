using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
  [SerializeField] private AudioSource collectionSoundEffect;
  [SerializeField] private Text scoreText;

  [SerializeField] private GameObject floatingPointsObj;

  private struct FloatingTextData
  {
    public string text;
    public Transform playerTransform;
    public float delay;

    public FloatingTextData(string text, Transform playerTransform, float delay)
    {
      this.text = text;
      this.playerTransform = playerTransform;
      this.delay = delay;
    }
  }

  private Queue<FloatingTextData> floatingTextQueue = new Queue<FloatingTextData>();
  private bool isProcessingQueue = false;

  private void Start()
  {
    UpdateUI();
  }

  public void AddCherries(int cherries)
  {
    collectionSoundEffect.Play();
    Scoring.totalCherries += cherries;
    int points = cherries * 100;
    AddScore(points);
  }

  public void AddMelons(int melons)
  {
    collectionSoundEffect.Play();
    Scoring.totalMelons += melons;
    int points = melons * 500;
    AddScore(points);
    EnqueueFloatingText("+1UP", this.transform, 0.3f);
  }

  public void AddStrawberries(int strawberries)
  {
    collectionSoundEffect.Play();
    Scoring.totalMelons += strawberries;
    int points = strawberries * 500;
    AddScore(points);
    EnqueueFloatingText("HP+", this.transform, 0.3f);
  }

  public void AddPineapples(int pineapples)
  {
    collectionSoundEffect.Play();
    Scoring.totalMelons += pineapples;
    int points = pineapples * 500;
    AddScore(points);
    EnqueueFloatingText("++", this.transform, 0.3f);
  }

  public void AddKills(int kills)
  {
    Scoring.totalKills += kills;
    int points = kills * 200;
    AddScore(points);
  }

  public void AddLevels(int levels)
  {
    Scoring.totalLevel += levels;
    int points = levels * 1000;
    AddScore(points);
    EnqueueFloatingText("FINISH!", this.transform, 0.3f);
  }

  public void AddCheckpoint(int check)
  {
    int points = check * 500;
    AddScore(points);
    EnqueueFloatingText("CHECKPOINT", this.transform, 0.3f);
  }

  public void AddScore(int points)
  {
    Scoring.totalScore += points;
    ShowFloatingPoints(points, this.transform);
    UpdateUI();
  }

  private void UpdateUI()
  {
    if (scoreText != null)
    {
      scoreText.text = "Score: " + Scoring.totalScore;
    }
  }

  private void ShowFloatingPoints(int points, Transform playerTransform)
  {
    EnqueueFloatingText(points.ToString(), playerTransform, 0);
  }

  private void EnqueueFloatingText(string text, Transform playerTransform, float delay)
  {
    floatingTextQueue.Enqueue(new FloatingTextData(text, playerTransform, delay));

    if (!isProcessingQueue)
    {
      StartCoroutine(ProcessFloatingTextQueue());
    }
  }

  private IEnumerator ProcessFloatingTextQueue()
  {
    isProcessingQueue = true;

    while (floatingTextQueue.Count > 0)
    {
      var floatingTextData = floatingTextQueue.Dequeue();
      yield return new WaitForSeconds(floatingTextData.delay);

      GameObject pointsPopup = Instantiate(floatingPointsObj, floatingTextData.playerTransform.position, Quaternion.identity);
      FloatingPoints floatingPoints = pointsPopup.GetComponent<FloatingPoints>();
      floatingPoints.Initialize(floatingTextData.playerTransform, floatingTextData.text);

      yield return new WaitForSeconds(0.2f);
    }

    isProcessingQueue = false;
  }

}

