using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform leftDoor;  // Assign the left door in Unity
    [SerializeField] private Transform rightDoor; // Assign the right door in Unity
    [SerializeField] private float doorMoveSpeed = 2f; // Speed of door movement

    private Vector3 leftClosedPosition;
    private Vector3 leftOpenPosition;
    private Vector3 rightClosedPosition;
    private Vector3 rightOpenPosition;

    private bool isMoving = false;
    private bool isOpen = false;

    // Store the floor level for doors
    private float currentFloorY;
    private float initialY;

    void Start()
    {
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("Left or Right door not assigned!");
            return;
        }

        // Store the closed positions
        leftClosedPosition = leftDoor.position;
        rightClosedPosition = rightDoor.position;

        // Store the open positions (where the doors should move to)
        leftOpenPosition = new Vector3(leftDoor.position.x - 2.3f, leftDoor.position.y, leftDoor.position.z);  // Move left door left
        rightOpenPosition = new Vector3(rightDoor.position.x + 2.3f, rightDoor.position.y, rightDoor.position.z); // Move right door right

        // Store the initial vertical position of the doors
        initialY = leftDoor.position.y;
        currentFloorY = initialY;  // Start with the initial Y position
    }

    public void OpenDoors()
    {
        if (!isMoving) // Ensure doors are not already moving
        {
            StartCoroutine(MoveDoors(leftOpenPosition, rightOpenPosition));
        }
        else
        {
            Debug.Log("Doors are already moving. Cannot open.");
        }
    }

    public void CloseDoors()
    {
        if (!isMoving) // Ensure doors are not already moving
        {
            StartCoroutine(MoveDoors(leftClosedPosition, rightClosedPosition));
        }
        else
        {
            Debug.Log("Doors are already moving. Cannot close.");
        }
    }

    public void ToggleDoors()
    {
        if (isOpen)
        {
            CloseDoors();
        }
        else
        {
            OpenDoors();
        }
    }

    public bool IsDoorMoving()
    {
        return isMoving; // This checks if the doors are still in motion
    }

    private IEnumerator MoveDoors(Vector3 leftTarget, Vector3 rightTarget)
    {
        isMoving = true;
        Debug.Log("Doors started moving. Target: " + (leftTarget == leftOpenPosition ? "Open" : "Closed"));

        leftTarget.y = currentFloorY;
        rightTarget.y = currentFloorY;

        while (Vector3.Distance(leftDoor.position, leftTarget) > 0.001f || Vector3.Distance(rightDoor.position, rightTarget) > 0.001f)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTarget, doorMoveSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTarget, doorMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure final positions are exact
        leftDoor.position = leftTarget;
        rightDoor.position = rightTarget;

        // Fix: Compare distances instead of checking direct equality
        isOpen = Vector3.Distance(leftDoor.position, leftOpenPosition) < 0.01f && Vector3.Distance(rightDoor.position, rightOpenPosition) < 0.01f;
        isMoving = false;
        Debug.Log($"Doors stopped moving. isOpen: {isOpen}");
        isOpen = true;
        Debug.Log($"Doors stopped moving. isOpen: {isOpen}");
    }


    // Call this to set the current floor level when elevator reaches a new floor
    public void SetCurrentFloorY(float newY)
    {
        // Add the Y offset for floors above the first floor
        //if (newY > 0f)
        //{
          //  newY += 2.16f; // Offset for floors above the first floor
        //}
        newY += 2.16f;
        // Update the current floor level
        currentFloorY = newY;
    }

    // To reset back to the initial Y position when needed
    public void ResetDoorPosition()
    {
        leftDoor.position = new Vector3(leftDoor.position.x, initialY, leftDoor.position.z);
        rightDoor.position = new Vector3(rightDoor.position.x, initialY, rightDoor.position.z);
        currentFloorY = initialY;  // Reset the door's current floor Y to initial
    }

    public bool IsOpen()
    {
        return isOpen; // This checks if the doors are currently open
    }
}