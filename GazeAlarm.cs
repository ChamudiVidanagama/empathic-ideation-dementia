using UnityEngine;

public class GazeAlarm : MonoBehaviour
{
    public AudioClip alarmSound; // Assign in the Inspector
    private AudioSource audioSource;
    private Transform playerCamera;
    private float gazeTimer = 0f;
    private bool isLooking = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = alarmSound;
        audioSource.loop = false;

        playerCamera = Camera.main.transform; // XR Camera
    }

    void Update()
    {
        Ray gazeRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(gazeRay, out hit))
        {
            // Check if the hit object is one of the colliders under "Colliders"
            Transform hitParent = hit.collider.transform.root;

            if (hitParent == transform) // If looking at this car
            {
                if (!isLooking)
                {
                    isLooking = true;
                    gazeTimer = 0f; // Reset timer when gaze starts
                }

                gazeTimer += Time.deltaTime;

                if (gazeTimer >= 60.0f && !audioSource.isPlaying)
                {
                    audioSource.Play(); // Trigger alarm after 60s
                    Debug.Log("Alarm Triggered on " + gameObject.name);
                }
            }
            else
            {
                ResetGaze();
            }
        }
        else
        {
            ResetGaze();
        }
    }

    void ResetGaze()
    {
        isLooking = false;
        gazeTimer = 0f;
    }
}
