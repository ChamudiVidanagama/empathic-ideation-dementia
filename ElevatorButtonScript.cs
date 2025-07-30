using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ElevatorButtonScript : MonoBehaviour
{
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;

    public void OnPress()
    {
        if (leftDoorAnimator != null && rightDoorAnimator != null)
        {
            leftDoorAnimator.SetTrigger("Open");
            rightDoorAnimator.SetTrigger("Open");
        }
        else
        {
            Debug.LogError("Elevator door animators are not assigned!");
        }
    }
}
