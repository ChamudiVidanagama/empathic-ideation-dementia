using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour
{
    public AudioClip defaultBackgroundSound; // Floors 1, 3, 4, 5
    public AudioClip specialBackgroundSound; // Floor 2

    private AudioSource audioSource;
    private Transform playerTransform;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Get the player's transform (XR Rig)
        playerTransform = transform;

        // Ensure AudioSource is set up correctly
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on XR Rig!");
            return;
        }

        // Start playing the initial sound
        UpdateBackgroundSound();
    }

    void Update()
    {
        UpdateBackgroundSound();
    }

    void UpdateBackgroundSound()
    {
        float playerHeight = playerTransform.position.y;

        // If player is below the 1st floor or above the 5th floor, stop audio
        if (playerHeight < -1.0f || playerHeight > 12.0f)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Muted: Player is outside the main floors.");
            }
            return;
        }

        AudioClip newClip = defaultBackgroundSound; // Default sound for most floors

        // Assign different sound for floor 2
        if (playerHeight >= 2.0f && playerHeight < 5.0f)
        {
            newClip = specialBackgroundSound;
        }

        // Change sound only if a new one is detected
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
            Debug.Log("Now playing: " + newClip.name);
        }
    }
}
