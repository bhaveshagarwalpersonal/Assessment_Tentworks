// Order.cs
using System.Collections.Generic;
public class Order
{
    public List<IngredientType> requirements = new List<IngredientType>();
    public List<bool> delivered = new List<bool>();
    public float startTime;

    public Order(List<IngredientType> items)
    {
        requirements = new List<IngredientType>(items);
        delivered = new List<bool>(new bool[items.Count]);
        startTime = UnityEngine.Time.time;
    }

    // Try to fulfill an ingredient; return true if accepted
    public int TryAdd(Ingredient ing)
    {
        if (!ing.IsReady()) return -1;

        for (int i = 0; i < requirements.Count; i++)
        {
            if (!delivered[i] && requirements[i] == ing.data.type)
            {
                delivered[i] = true;
                return i; // return matched slot index
            }
        }

        return -1;
    }

    public bool IsComplete()
    {
        foreach (bool done in delivered) if (!done) return false;
        return true;
    }

    public bool HasBurnt() { /* checks if any required item was burnt; optional */ return false; }
}
