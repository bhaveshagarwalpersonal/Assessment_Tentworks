using UnityEngine;

[CreateAssetMenu(menuName = "Game/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public IngredientType type;

    public int scoreValue;

    public bool requiresChop;
    public bool requiresCook;

    public float cookTime;
    public float burnOffset;

    public Sprite icon; // for UI
}

public enum IngredientType
{
    Vegetable,
    Meat,
    Cheese
}
