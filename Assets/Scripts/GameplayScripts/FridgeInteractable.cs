// FridgeInteractable.cs
using UnityEngine;
public class FridgeInteractable : Interactable
{
    public GameObject ingredientPrefab;
    public IngredientData data; // assign via inspector

    public override void PressedQ(PlayerController player)
    {
        if (player.currentIngredient != null) return;
        GameObject obj = Instantiate(ingredientPrefab);
        Ingredient ing = obj.GetComponent<Ingredient>();
        ing.data = data;
        player.PickIngredient(ing);
    }
}
