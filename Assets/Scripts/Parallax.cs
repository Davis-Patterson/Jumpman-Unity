using System.Collections;
using System.Numerics;
using UnityEngine;

public class Parallax : MonoBehaviour
{
  Material mat;
  float distance;

  [Range(0f, 0.5f)]
  public float speed = 0.2f;

  void Start()
  {
    mat = GetComponent<Renderer>().material;
  }

  void Update()
  {
    distance += Time.deltaTime * speed;
    mat.SetTextureOffset("_MainTex", UnityEngine.Vector2.right * distance);
  }
}