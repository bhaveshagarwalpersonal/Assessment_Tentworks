// DeliveryCounterInteractable.cs
using UnityEngine;

public class DeliveryCounterInteractable : Interactable
{
    public Order order;          // Assigned by OrderManager
    public int counterIndex;     // 0..3 identifying which counter
    public bool done = false;

    public override void PressedQ(PlayerController player)
    {
        if (player.currentIngredient == null || done) return;
        Ingredient ing = player.currentIngredient;
        int deliveredIndex = order.TryAdd(ing);

        if (deliveredIndex != -1)
        {
            Destroy(ing.gameObject);
            player.currentIngredient = null;
            // Update UI: OrderManager/UIManager will update icons
            OrderManager.Instance.OnIngredientDelivered(counterIndex, deliveredIndex);
            if (order.IsComplete())
            {
                done = true;
                OrderManager.Instance.OnOrderComplete(counterIndex);
            }
        }
        else
        {
            // If ingredient was Burnt or wrong type, consider failure
            // Optionally respawn order after delay
        }
    }
}
