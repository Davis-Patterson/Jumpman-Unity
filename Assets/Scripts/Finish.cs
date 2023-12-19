using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
  private AudioSource finishSound;

  private bool levelCompleted = false;

  private ScoreManager scoreManager;

  void Start()
  {
    finishSound = GetComponent<AudioSource>();
    scoreManager = FindObjectOfType<ScoreManager>();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
  if (collision.gameObject.name == "Player" && !levelCompleted)
  {
    scoreManager.AddLevels(1);
    finishSound.Play();
    levelCompleted = true;
    Invoke("CompleteLevel", 1.5f);
    }
  }

  private void CompleteLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }
}
