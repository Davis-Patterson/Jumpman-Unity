using UnityEngine;

public class ChamTongue : MonoBehaviour
{
  private EdgeCollider2D tongueCollider;

  void Start()
  {
    tongueCollider = GetComponent<EdgeCollider2D>();
  }

  public void ActivateTongue(bool isActive)
  {
    Debug.Log("Tongue is now " + (isActive ? "Active" : "Inactive"));
    tongueCollider.enabled = isActive;
  }

}
