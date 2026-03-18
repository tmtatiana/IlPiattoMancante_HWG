using UnityEngine;

public class YouLoseScreen : MonoBehaviour
{
    public AudioClip youLoseSound;
    private AudioSource audioSource;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (audioSource != null && youLoseSound != null)
        {
            audioSource.PlayOneShot(youLoseSound);
        }
    }
}
