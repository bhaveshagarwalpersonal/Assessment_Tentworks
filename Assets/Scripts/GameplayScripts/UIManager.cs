// UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI timerText, scoreText,gameOverScoreText, highScoreText;
    public OrderUI[] orderUI; // references to UI for each counter
    public Canvas endScreenCanvas;

    [Header("IconsSprites")]

    public Sprite tick;
    public Sprite vegtable_Chopped;
    public Sprite meat_Cooked;
    public Sprite cheese_Raw;
    void Awake() { Instance = this; }

    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateScoreDisplay(int current, int high)
    {
        scoreText.text = current.ToString();
        gameOverScoreText.text = current.ToString();
        highScoreText.text = high.ToString();
    }

    public void ShowEndScreen()
    {
        endScreenCanvas.gameObject.SetActive(true);
        // Optional: animate in, e.g. endScreenCanvas.DOFade(1, 1f);
    }

    public Sprite GetIconImage(IngredientType ingredientType) 
    {
        if (ingredientType == IngredientType.Cheese)
        {
            return cheese_Raw;
        }
        else if (ingredientType == IngredientType.Vegetable)
        {
            return vegtable_Chopped;

        }
        else if (ingredientType == IngredientType.Meat)
        {
            return meat_Cooked;

        }
        else 
        {
            return null;

        }
    }
}
