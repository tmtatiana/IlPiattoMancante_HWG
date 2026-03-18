using UnityEngine;

public class YouWinRoom : MonoBehaviour
{
    public AudioClip youWinSound;
    private AudioSource audioSource;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (audioSource != null && youWinSound != null)
        {
            audioSource.PlayOneShot(youWinSound);
        }
    }
}
