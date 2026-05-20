// OrderUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
// Manages the world-space UI icons for a single order
public class OrderUI : MonoBehaviour
{
    public Sprite tickSprite;
    public Transform iconContainer; // parent with HorizontalLayoutGroup
    private List<Image> icons = new List<Image>();
    public GameObject popUpPlaceHolder;

    // Initialize UI when order spawns
    public void SetupOrderUI(Order order)
    {
        // Clear previous icons
        foreach (Transform child in iconContainer) Destroy(child.gameObject);
        icons.Clear();

        foreach (IngredientType type in order.requirements)
        {
            GameObject iconGO = new GameObject("Icon", typeof(Image));
            iconGO.transform.SetParent(iconContainer, false);
            Image img = iconGO.GetComponent<Image>();
            img.sprite = UIManager.Instance.GetIconImage(type);
            icons.Add(img);
        }
    }

    // Mark the next icon as delivered (tick)
    public void MarkDelivered(int index)
    {
        if (index < 0 || index >= icons.Count) return;

        icons[index].sprite = tickSprite;
    }

    public void ShowScorePopup(string score)
    {
        // Get Components
        RectTransform rect = popUpPlaceHolder.GetComponent<RectTransform>();
        CanvasGroup canvas = popUpPlaceHolder.GetComponent<CanvasGroup>();
        TMPro.TMP_Text txt = popUpPlaceHolder.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

        // Kill previous tweens
        rect.DOKill();
        canvas.DOKill();

        // Store original values
        Vector2 originalPos = rect.anchoredPosition;
        Vector3 originalScale = Vector3.one;

        // Reset state
        rect.anchoredPosition = originalPos;
        rect.localScale = Vector3.zero;
        canvas.alpha = 0f;

        // Set score text
        txt.text = score;

        // Color based on score
        txt.color = score.Contains("-") ? Color.red : Color.green;

        // Create animation sequence
        DG.Tweening.Sequence seq = DG.Tweening.DOTween.Sequence();

        // Fade in + Pop scale
        seq.Append(
            canvas.DOFade(1f, 0.15f)
        );

        seq.Join(
            rect.DOScale(originalScale, 0.25f)
                .SetEase(DG.Tweening.Ease.OutBack)
        );

        // Hold visible for a bit
        seq.AppendInterval(0.6f);

        // Move upward smoothly
        seq.Append(
            rect.DOAnchorPosY(originalPos.y + 120f, 1.2f)
                .SetEase(DG.Tweening.Ease.OutQuad)
        );

        // Fade out while moving
        seq.Join(
            canvas.DOFade(0f, 1.2f)
        );

        // Reset everything after complete
        seq.OnComplete(() =>
        {
            rect.anchoredPosition = originalPos;
            rect.localScale = originalScale;
            canvas.alpha = 0f;
        });
    }
}
