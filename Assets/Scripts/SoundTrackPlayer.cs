using UnityEngine;

public class SoundTrackPlayer : MonoBehaviour
{
    public AudioClip music;
    [Range(0f, 1f)] public float volume;

    private static SoundTrackPlayer instance;
    private AudioSource audioSource;

    void Awake()
    {
        
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1f);
        }
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
        ChangeVolume(PlayerPrefs.GetFloat("Volume"));
    }

    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("Music"));
        if (audioSource.clip != null && !audioSource.isPlaying && PlayerPrefs.GetInt("Music") == 1)
            audioSource.Play();
    }

    public static void CheckMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
            instance.audioSource.Play();
        else if (PlayerPrefs.GetInt("Music") == 0)
            instance.audioSource.Stop();
    }
    public static void ChangeVolume(float newVolume)
    {
        instance.audioSource.volume = newVolume;
    }

    public void Play() => audioSource.Play();
    public void Stop() => audioSource.Stop();
}