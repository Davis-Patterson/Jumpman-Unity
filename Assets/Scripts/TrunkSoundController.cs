using UnityEngine;

public class TrunkSoundController : MonoBehaviour
{
  public Transform player;
  public AudioSource shootingSound;
  public float maxVolumeDistance = 5.0f;
  public float minVolumeDistance = 30.0f;

  void Start()
  {
    shootingSound = GetComponent<AudioSource>();
  }

  void Update()
  {
    float distance = Vector3.Distance(player.position, transform.position);

    float volume = 1.0f - Mathf.Clamp01((distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));

    shootingSound.volume = volume;
  }
}
