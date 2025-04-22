using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // Starts music
        }
    }
}