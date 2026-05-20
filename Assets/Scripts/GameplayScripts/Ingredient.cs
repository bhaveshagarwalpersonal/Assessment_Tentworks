// Ingredient.cs
using UnityEngine;
public enum IngredientState { Raw, Chopped, Cooked, Burnt }

public class Ingredient : MonoBehaviour
{
    [HideInInspector] public IngredientData data;
    [HideInInspector] public IngredientState currentState = IngredientState.Raw;

    public GameObject rawModel, choppedModel, cookedModel, burntModel;
    private float timer;
    private bool isProcessing;
    public GameObject processVFX;
    public GameObject extraVFX;

    private void Start()
    {
        timer = 0;
    }

    void Update()
    {
        if (!isProcessing) return;
        timer += Time.deltaTime; // frame time【15†L79-L87】

        // Chop (instant 2s chop)
        if (data.requiresChop && currentState == IngredientState.Raw && timer >= 2f)
        {
            currentState = IngredientState.Chopped;
            isProcessing = false;
            UpdateVisual();
        }
        // Cook and Burn
        if (data.requiresCook && currentState != IngredientState.Burnt)
        {
            if (currentState == IngredientState.Raw && timer >= data.cookTime)
            {
                currentState = IngredientState.Cooked;
                UpdateVisual();
            }
            if (timer >= data.cookTime + data.burnOffset)
            {
                currentState = IngredientState.Burnt;
                isProcessing = false;
                UpdateVisual();
                if (extraVFX != null)
                {
                    extraVFX.SetActive(true);
                }
            }
        }
    }

    // Called to begin chopping or cooking
    public void StartProcessing()
    {
        if ((data.requiresChop && currentState == IngredientState.Raw) ||
            (data.requiresCook && currentState == IngredientState.Raw) ||
            (data.requiresCook && currentState == IngredientState.Cooked))
        {
           // timer = 0f;
            isProcessing = true;
            if (processVFX != null) 
            {
                processVFX.SetActive(true);
            }
        }
    }

    public void StopProcessing() 
    {
        isProcessing = false;
        if (processVFX != null)
        {
            processVFX.SetActive(false);
        }
    }

    // Swap active model based on state
    private void UpdateVisual()
    {
        rawModel.SetActive(currentState == IngredientState.Raw);
        choppedModel.SetActive(currentState == IngredientState.Chopped);
        cookedModel.SetActive(currentState == IngredientState.Cooked);
        burntModel.SetActive(currentState == IngredientState.Burnt);
    }

    // Check if ingredient is in final ready state (not including Burnt)
    public bool IsReady()
    {
        if (currentState == IngredientState.Burnt) return false;
        if (data.requiresChop && currentState != IngredientState.Chopped) return false;
        if (data.requiresCook && currentState != IngredientState.Cooked) return false;
        return true;
    }
}
