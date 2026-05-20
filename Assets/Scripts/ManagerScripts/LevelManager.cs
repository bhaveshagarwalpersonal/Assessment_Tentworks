// LevelManager.cs
using UnityEngine;
using DG.Tweening; // For any UI tweens (optional)

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public float gameTime = 10f; // 3 minutes

    private void OnEnable()
    {
        Instance = this;
        Time.timeScale = 1f;
        ResetGame();
    }

    private float timer;
    void ResetGame()
    {
        timer = gameTime;
        ScoreManager.Instance.ResetScore();
        OrderManager.Instance.StartOrders();
        UIManager.Instance.UpdateScoreDisplay(0, ScoreManager.Instance.GetHighScore());
    }

    void Update()
    {
        timer -= Time.deltaTime;
        UIManager.Instance.UpdateTimer(timer);
        if (timer <= 0f)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowEndScreen();
    }
}
