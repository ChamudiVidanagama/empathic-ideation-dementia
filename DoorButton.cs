using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorButton : MonoBehaviour
{
    public DoorController doorController; // Assign this in Unity
    private XRSimpleInteractable interactable;

    void Start()
    {
        if (TryGetComponent(out interactable))
        {
            interactable.selectEntered.AddListener(OnButtonPressXR);
        }
    }

    // XR interaction method
    private void OnButtonPressXR(SelectEnterEventArgs args)
    {
        OnButtonPress();  // Calls the UI-compatible method
    }

    // Works with UI buttons too
    public void OnButtonPress()
    {
        if (doorController != null)
        {
            doorController.ToggleDoors(); // Call the toggle function
        }
        else
        {
            Debug.LogError("DoorController script not assigned to DoorButton!");
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
