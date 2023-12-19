using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text cherriesText;
    [SerializeField] private Text melonsText;

    private void Start()
    {
        if (levelText != null)
            levelText.text = "Level: " + Scoring.totalLevel;

        if (scoreText != null)
            scoreText.text = "Score: " + Scoring.totalScore;

        if (cherriesText != null)
            cherriesText.text = "Cherries: " + Scoring.totalCherries;

        if (melonsText != null)
            melonsText.text = "Melons: " + Scoring.totalMelons;
    }
}
