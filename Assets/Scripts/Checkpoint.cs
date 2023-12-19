using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
  private SpawnMaster gm;
  private AudioSource checkpointSound;
  private Animator anim;

  private ScoreManager scoreManager;

  void Start()
  {
    gm = GameObject.FindGameObjectWithTag("SpawnMaster").GetComponent<SpawnMaster>();
    checkpointSound = GetComponent<AudioSource>();
    anim = GetComponent<Animator>();
    scoreManager = FindObjectOfType<ScoreManager>();
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      if (gm.lastCheckpointPos != (Vector2)transform.position)
      {
        if (gm.lastCheckpointPos != (Vector2)transform.position)
        {
          gm.lastCheckpointPos = transform.position;
          checkpointSound.Play();
          anim.SetTrigger("checked");
          scoreManager.AddCheckpoint(1);
        }
      }
    }
  }
}
