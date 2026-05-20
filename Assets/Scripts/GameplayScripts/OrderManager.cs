// OrderManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public DeliveryCounterInteractable[] counters; // assign 4 counters via inspector
    public OrderUI[] orderUI; // references to 4 world-space canvases

    void Awake() { Instance = this; }

    public void StartOrders()
    {
        for (int i = 0; i < counters.Length; i++)
            SpawnOrder(i);
    }

    public void SpawnOrder(int index)
    {
        // Generate random order of 2-3 ingredients (duplicates allowed)
        int count = Random.Range(2, 4); // 2 or 3
        List<IngredientType> items = new List<IngredientType>();
        for (int j = 0; j < count; j++)
        {
            // Assuming IngredientType enum matches data names
            IngredientType type = (IngredientType)Random.Range(0, 3);
            items.Add(type);
        }
        Order newOrder = new Order(items);
        counters[index].order = newOrder;
        orderUI[index].SetupOrderUI(newOrder);
        counters[index].done = false;
    }

    public void OnIngredientDelivered(int counterIndex, int ingredientIndex)
    {
        orderUI[counterIndex].MarkDelivered(ingredientIndex);
    }

    public void OnOrderComplete(int counterIndex)
    {
        Order ord = counters[counterIndex].order;
        OrderUI ordUI = orderUI[counterIndex];

        int sumValue = 0;

        foreach (var req in ord.requirements)
        {
            switch (req)
            {
                case IngredientType.Vegetable:
                    sumValue += 20;
                    break;

                case IngredientType.Meat:
                    sumValue += 30;
                    break;

                case IngredientType.Cheese:
                    sumValue += 10;
                    break;
            }
        }

        float timeTaken = Time.time - ord.startTime;

        Debug.Log("Time Taken : " + timeTaken);

        int score = Mathf.Max(0,
            sumValue - Mathf.FloorToInt(timeTaken));

        //score = sumValue;

        ScoreManager.Instance.AddScore(score);
        Debug.Log("Score Added : " + score);

        if (score > 0)
        {
            ordUI.ShowScorePopup("+" + score.ToString());
        } else if (score < 0) { ordUI.ShowScorePopup("-" + score.ToString()); } else { ordUI.ShowScorePopup(score.ToString()); }

        StartCoroutine(RespawnOrderAfterDelay(counterIndex));
    }

    private IEnumerator RespawnOrderAfterDelay(int index)
    {
        yield return new WaitForSeconds(5f);
        SpawnOrder(index);
    }
}
