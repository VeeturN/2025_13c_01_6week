using UnityEngine;

public class SoundTrackPlayer : MonoBehaviour
{
    public AudioClip music;
    [Range(0f, 1f)] public float volume = 1f;

    private static SoundTrackPlayer instance;
    private AudioSource audioSource;

    void Awake()
    {
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
    }

    void Start()
    {
        if (audioSource.clip != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    public void Play() => audioSource.Play();
    public void Stop() => audioSource.Stop();
}