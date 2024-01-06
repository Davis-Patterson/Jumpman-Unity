using UnityEngine;

public class FallBounds : MonoBehaviour
{
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
      if (playerLife != null)
      {
        playerLife.Die();
      }
    }
  }
}
