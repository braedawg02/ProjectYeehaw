using UnityEngine;

public enum SoundType
{
    Music,
    Footsteps,
    Hurt,
    Death,
    
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] FootsetpsList;
    private static SoundManager instance;
    private AudioSource audioSource;
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        AudioClip clipToPlay;
        
        if (sound == SoundType.Footsteps)
        {
            int randomIndex = Random.Range(0, instance.FootsetpsList.Length);
            clipToPlay = instance.FootsetpsList[randomIndex];
        }
        else
        {
            clipToPlay = instance.soundList[(int)sound];
        }
        instance.audioSource.PlayOneShot(clipToPlay, volume);
    }
}
