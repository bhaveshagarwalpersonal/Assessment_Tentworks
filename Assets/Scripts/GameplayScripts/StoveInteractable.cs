// StoveInteractable.cs
using UnityEngine;

public class StoveInteractable : Interactable
{
    public Transform[] slots = new Transform[2];
    private Ingredient[] items = new Ingredient[2];

    public override void PressedQ(PlayerController player)
    {
        // Place in first empty slot
        if (player.currentIngredient != null)
        {
            for (int i = 0; i < 2; i++)
            {
                if (items[i] == null)
                {
                    items[i] = player.currentIngredient;
                    player.DropIngredient();
                    items[i].transform.SetParent(slots[i]);
                    items[i].transform.localPosition = Vector3.zero;
                    break;
                }
            }
        }
        // Pick up from topmost occupied slot
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if (items[i] != null)
                {
                    player.PickIngredient(items[i]);
                    items[i].StopProcessing();
                    items[i] = null;
                    break;
                }
            }
        }
    }

    public override void PressedE(PlayerController player)
    {
        foreach (var ing in items)
        {
            if (ing != null && ing.data.requiresCook)
            {
                ing.StartProcessing();
            }
        }
    }
}
