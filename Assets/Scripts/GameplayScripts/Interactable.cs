// Interactable.cs
using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    public GameObject highLighterObj;
    // Called when player presses Q while targeting this object
    public virtual void PressedQ(PlayerController player) 
    {
        if (highLighterObj != null)
        {
            highLighterObj.SetActive(false);
        }
    }
    // Called when player presses E
    public virtual void PressedE(PlayerController player) 
    {
        if (highLighterObj != null)
        {
            highLighterObj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && highLighterObj != null) 
        {
            highLighterObj.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && highLighterObj != null)
        {
            highLighterObj.SetActive(false);
        }
    }
}
