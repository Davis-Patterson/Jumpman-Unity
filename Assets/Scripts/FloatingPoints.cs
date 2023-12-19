using UnityEngine;
using System.Collections;

public class FloatingPoints : MonoBehaviour
{
    public float destroyTime = 3f;
    public Vector3 offset = new Vector3(1.5f, 1, 0);
    private Transform playerTransform;
    private TextMesh pointsTextMesh;

    void Awake()
    {
        pointsTextMesh = GetComponentInChildren<TextMesh>();
        gameObject.SetActive(false);
    }

    public void Initialize(Transform player, string text)
    {
        playerTransform = player;
        transform.position = playerTransform.position + offset;
        SetText(text);

        gameObject.SetActive(true);

        StartCoroutine(HideAfterDelay(destroyTime));
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        gameObject.SetActive(true);
        if (pointsTextMesh != null)
        {
            pointsTextMesh.text = text;
        }
    }
}
