// TrashInteractable.cs
using UnityEngine;
public class TrashInteractable : Interactable
{
    public override void PressedQ(PlayerController player)
    {
        if (player.currentIngredient == null) return;
        Destroy(player.currentIngredient.gameObject);
        player.currentIngredient = null;
    }
}
