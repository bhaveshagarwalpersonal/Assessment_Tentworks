// PlayerController.cs
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public Transform holdPoint;
    public float moveSpeed = 5f;
    [HideInInspector] public Ingredient currentIngredient;
    private Rigidbody rb;
    private Vector3 inputDir;
    private bool frozen = false;

    public Interactable currentInteractable;


    void Awake() { rb = GetComponent<Rigidbody>(); }

    void Update()
    {
        // Movement input
        if (!frozen)
        {
            inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
        else
        {
            inputDir = Vector3.zero;
        }

        // Interaction input
        if (Input.GetKeyDown(KeyCode.Q) && currentInteractable != null)
        {
            Interactable interactable = currentInteractable;
            interactable?.PressedQ(this);
        }
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            Interactable interactable = currentInteractable;
            interactable?.PressedE(this);
        }
    }

    void FixedUpdate()
    { 
        if (frozen) return;
        // ----- Movement (original) -----
        rb.MovePosition(rb.position + inputDir * moveSpeed * Time.fixedDeltaTime);

        // ----- Rotation toward movement direction -----
        if (inputDir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0, targetAngle, 0);
            // Rotate the Rigidbody itself (or use model.transform if you prefer)
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, 540f * Time.fixedDeltaTime));
        }

        // ----- State detection and function calls -----
        bool wasMoving = false;  // persists across calls (no class field needed)
        bool isMoving = inputDir.magnitude > 0.1f;

        // Transition: moving → stopped
        if (wasMoving && !isMoving)
        {
            OnStopping();
        }

        // State-specific continuous calls
        if (isMoving)
        {
            OnMoving();   // called every frame while moving
        }
        else
        {
            OnIdle();     // called every frame while idle
        }

        wasMoving = isMoving;
    }

    // ----- Three methods to implement your own logic -----
    private void OnMoving()
    {
        // Called every frame while the player is moving
        // e.g., play footsteps, emit dust particles, etc.
    }

    private void OnIdle()
    {
        // Called every frame while the player is stationary
        // e.g., idle breathing, stand animation, etc.
    }

    private void OnStopping()
    {
        // Called exactly once when the player stops moving
        // e.g., play "skid" sound, stop running particles, etc.
    }

    public void PickIngredient(Ingredient ing)
    {
        if (currentIngredient != null) return;
        currentIngredient = ing;
        ing.transform.SetParent(holdPoint);
        ing.transform.localPosition = Vector3.zero;
        ing.transform.localRotation = Quaternion.identity;
    }

    public void DropIngredient()
    {
        if (currentIngredient == null) return;
        currentIngredient.transform.SetParent(null);
        currentIngredient = null;
    }

    public void FreezePlayer(bool freeze)
    {
        frozen = freeze;
    }

    // Raycast or trigger detection to find current interactable under player
    private Interactable GetCurrentInteractable()
    {
        float radius = 1f; // adjust as needed
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in hits) // loop through all nearby colliders
        {
            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null)
            {
                Debug.Log($"Found interactable: {hit.gameObject.name}");
                return interactable; // return the first one found
            }
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
        {
            currentInteractable = interactable;
          //  Debug.Log($"Entered interactable: {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;
         //   Debug.Log($"Exited interactable: {other.gameObject.name}");
        }
    }

}
