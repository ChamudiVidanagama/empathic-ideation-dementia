using UnityEngine;
using System.Collections;

public class CarProximityChecker : MonoBehaviour
{
    public float detectionRadius = 1.0f;  // How close the player must be
    public float triggerTime = 45.0f;  // Time before alarm triggers
    public float alarmDuration = 30.0f; // How long the alarm plays
    public AudioClip alarmSound; // Assign an alarm sound in the Inspector

    private Transform player;
    private AudioSource audioSource;
    private float timeSpentNearCar = 0f;
    private bool alarmTriggered = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Ensure XR Rig has "Player" tag
        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if missing
        audioSource.clip = alarmSound;
        audioSource.loop = false;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRadius)
        {
            timeSpentNearCar += Time.deltaTime;

            if (!alarmTriggered && timeSpentNearCar >= triggerTime)
            {
                StartCoroutine(TriggerAlarm());
            }
        }
        else
        {
            timeSpentNearCar = 0f;
        }
    }

    IEnumerator TriggerAlarm()
    {
        alarmTriggered = true;
        audioSource.Play();
        yield return new WaitForSeconds(alarmDuration);
        audioSource.Stop();
        alarmTriggered = false;
        timeSpentNearCar = 0f; // Reset timer
    }
}
