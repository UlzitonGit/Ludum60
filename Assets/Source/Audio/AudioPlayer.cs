using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
    
}
