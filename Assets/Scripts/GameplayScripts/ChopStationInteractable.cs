// ChopStationInteractable.cs
using UnityEngine;
using System.Collections;

public class ChopStationInteractable : Interactable
{
    public Transform slot;
    private Ingredient current = null;

    public override void PressedQ(PlayerController player)
    {
        // Place
        if (current == null && player.currentIngredient != null)
        {
            current = player.currentIngredient;
            player.DropIngredient();
            current.transform.SetParent(slot);
            current.transform.localPosition = Vector3.zero;
        }
        // Pick up
        else if (current != null && player.currentIngredient == null)
        {
            player.PickIngredient(current);
            current = null;
        }
    }

    public override void PressedE(PlayerController player)
    {
        if (current == null) return;
        if (!current.data.requiresChop) return;
        StartCoroutine(DoChop(player));
    }

    private IEnumerator DoChop(PlayerController player)
    {
        player.FreezePlayer(true);
        current.StartProcessing();
        // Wait 2 seconds to simulate chopping
        yield return new WaitForSeconds(2f);
        player.FreezePlayer(false);
        current.StopProcessing();
    }
}
