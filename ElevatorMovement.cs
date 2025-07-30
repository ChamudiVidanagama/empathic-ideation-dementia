using UnityEngine;
using System.Collections;

public class ElevatorMovement : MonoBehaviour
{
    public DoorController doorController; // Assign this in Unity

    private Vector3[] floorPositions = new Vector3[] {
        new Vector3(6.22f, -0.68f, 5.26f),  // 1st floor
        new Vector3(6.22f, 2.32f, 5.26f),   // 2nd floor
        new Vector3(6.22f, 5.32f, 5.26f),   // 3rd floor
        new Vector3(6.22f, 8.32f, 5.26f),   // 4th floor
        new Vector3(6.22f, 11.32f, 5.26f)   // 5th floor
    };

    private int currentFloor = 0;
    private bool isMoving = false;
    public float moveSpeed = 5f;

    private Transform player;

    void Start()
    {
        // Ensure the elevator starts at the first floor position
        transform.position = floorPositions[currentFloor];

        // Set the initial door's vertical position (first floor Y)
        if (doorController != null)
        {
            doorController.SetCurrentFloorY(floorPositions[currentFloor].y);
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player (XR Setup) not found! Make sure it has the 'Player' tag.");
        }
    }

    public void CallElevator()
    {
        if (!isMoving)
        {
            // Start the elevator movement sequence
            StartCoroutine(ElevatorSequence());
        }
    }

    private IEnumerator ElevatorSequence()
    {
        isMoving = true;

        // Ensure the doors are fully closed before proceeding
        if (doorController != null)
        {
            if (doorController.IsOpen() || doorController.IsDoorMoving())
            {
                Debug.Log("Closing doors before elevator movement.");
                doorController.CloseDoors();
            }

            // Wait until the doors are fully closed
            while (doorController.IsDoorMoving())
            {
                Debug.Log("Waiting for doors to close...");
                yield return null;
            }

            // Ensure doors are properly marked as closed
            if (doorController.IsOpen())
            {
                Debug.LogError("Doors still detected as open! Forcing correction.");
                doorController.CloseDoors();  // Extra safety close
            }
        }

        // Step 2: Move the elevator to the next floor
        yield return StartCoroutine(MoveElevatorToNextFloor());

        // Step 3: Open the doors once the elevator reaches the floor
        if (doorController != null)
        {
            Debug.Log("Opening doors after reaching the floor.");
            doorController.OpenDoors();

            // Wait until doors are fully open
            while (doorController.IsDoorMoving())
            {
                yield return null;
            }
        }

        isMoving = false;
        Debug.Log("Elevator stopped moving.");
    }



    private IEnumerator MoveElevatorToNextFloor()
    {
        currentFloor = (currentFloor + 1) % floorPositions.Length;
        Vector3 targetPosition = floorPositions[currentFloor];

        // Move player with the elevator, ensuring they are within range
        if (player != null && IsPlayerOnElevator())
        {
            player.SetParent(transform);  // Attach player to elevator
        }

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        // After reaching the target floor, update the doors' position correctly
        if (doorController != null)
        {
            doorController.SetCurrentFloorY(targetPosition.y);
        }

        // Detach the player from the elevator once it has reached the target floor
        if (player != null)
        {
            player.SetParent(null);
        }

    }

    private bool IsPlayerOnElevator()
    {
        if (player != null)
        {
            float playerHeight = player.position.y;
            float elevatorHeight = transform.position.y;
            return Mathf.Abs(playerHeight - elevatorHeight) < 1.5f;
        }
        return false;
    }
}