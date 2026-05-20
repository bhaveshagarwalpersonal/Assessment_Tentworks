// ScoreManager.cs
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int currentScore = 0;
    public int highScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void AddScore(int value)
    {
        currentScore += value;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore); // save【13†L61-L69】
        }
        UIManager.Instance.UpdateScoreDisplay(currentScore, highScore);
    }

    public int GetHighScore() { return highScore; }
    public int GetCurrentScore() { return currentScore; }
    public void ResetScore() { currentScore = 0; }
}
