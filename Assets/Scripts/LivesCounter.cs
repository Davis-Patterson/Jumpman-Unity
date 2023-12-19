using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LivesCounter : MonoBehaviour
{
    [SerializeField] private float imageWidth = 100f;

    private RectTransform rect;
    public UnityEvent OutOfLives;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        OutOfLives.AddListener(GameOver);
    }

    private void Update()
    {
        AdjustImageWidth();
    }

    private void AdjustImageWidth()
    {
        int numOfLives = Scoring.totalLives;
        rect.sizeDelta = new Vector2(imageWidth * numOfLives, rect.sizeDelta.y);
    }

    public static void AddLife(int num = 1)
    {
        Scoring.totalLives += num;
    }

    public static void RemoveLife(int num = 1)
    {
        Scoring.totalLives -= num;
        if (Scoring.totalLives <= 0)
        {
            FindObjectOfType<LivesCounter>().GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(7);
    }
}
