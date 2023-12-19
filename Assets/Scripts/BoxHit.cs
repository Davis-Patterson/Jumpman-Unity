using System.Collections;
using UnityEngine;

public class BoxHit : MonoBehaviour
{
  public int maxHits = -1;
  public Sprite emptyBlock;

  [SerializeField] private AudioSource hitSound;
  [SerializeField] private AudioSource denySound;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      if (IsCollisionFromBelow(collision))
      {
        Hit();
      }
    }
  }

  private bool IsCollisionFromBelow(Collision2D collision)
  {
    float boxBottom = transform.position.y - (transform.localScale.y / 2);
    float playerTop = collision.transform.position.y + (collision.transform.localScale.y / 2);

    return playerTop < boxBottom;
  }

  private void Hit()
  {
    if (maxHits != 0)
    {

      hitSound.Play();
      maxHits--;

      Debug.Log("hit");
    }
    else
    {
      SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
      spriteRenderer.sprite = emptyBlock;
      denySound.Play();
    }
  }
}
