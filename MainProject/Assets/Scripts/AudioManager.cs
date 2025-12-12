using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource OTMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip OTMusicClip;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic()
    {
        if (musicClip == null || musicSource == null) return;

        if (musicSource.isPlaying && musicSource.clip == musicClip) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayOTMusic()
    {
        if (OTMusicClip == null || OTMusicSource == null) return;

        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        if (OTMusicSource.isPlaying && OTMusicSource.clip == OTMusicClip) return;

        OTMusicSource.clip = OTMusicClip;
        OTMusicSource.loop = true;
        OTMusicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }

    public void StopOTMusic()
    {
        if (OTMusicSource == null) return;
        OTMusicSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        
        sfxSource.PlayOneShot(clip, volume);
    }
}
