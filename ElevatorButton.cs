using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ElevatorButton : MonoBehaviour
{
    private ElevatorMovement elevatorMovement;
    private XRSimpleInteractable interactable;

    void Start()
    {
        elevatorMovement = FindObjectOfType<ElevatorMovement>();

        if (TryGetComponent(out interactable))
        {
            interactable.selectEntered.AddListener(OnButtonPressXR);
        }
    }

    // This method works with XR interactions
    private void OnButtonPressXR(SelectEnterEventArgs args)
    {
        OnButtonPress();  // Calls the UI-compatible method
    }

    // This method works with UI buttons
    public void OnButtonPress()
    {
        Debug.Log("Button Pressed! Calling Elevator...");

        if (elevatorMovement != null)
        {
            elevatorMovement.CallElevator();
        }
        else
        {
            Debug.LogError("ElevatorMovement script not found!");
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnButtonPressXR);
        }
    }
}
