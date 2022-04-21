using UnityEngine;

public class MusicBehaviour : MonoBehaviour
{
    [Header("Volume Adjust")]
    [SerializeField] float minVolume;
    [SerializeField] float maxVolume;

    AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.volume = maxVolume;
    }

    public void SetMusicVolume(bool isGamePaused)
    {
        if (isGamePaused)
        {
            music.volume = minVolume;
        }
        else
        {
            music.volume = maxVolume;
        }
    }
}
