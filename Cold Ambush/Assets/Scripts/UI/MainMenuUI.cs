using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip buttonPressed;

    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        _audioSource.PlayOneShot(buttonPressed);
        SceneManager.LoadScene(1);
    }

    public void SetMenuActive(GameObject menu)
    {
        _audioSource.PlayOneShot(buttonPressed);
        menu.SetActive(!menu.activeInHierarchy);
    }

    public void QuitGame()
    {
        _audioSource.PlayOneShot(buttonPressed);
        Application.Quit();
    }
}
