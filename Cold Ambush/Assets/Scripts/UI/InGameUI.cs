using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField] Toggle invertControlsToggle;
    [Header("Audio Clips")]
    [SerializeField] AudioClip buttonPressed;

    PauseGame _pauseGame;
    AudioSource _audioSource;
    PlayerControls _playerControls;

    // Start is called before the first frame update
    void Start()
    {
        _pauseGame = FindObjectOfType<PauseGame>();
        _audioSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
        _playerControls = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
    }

    public void InvertControls()
    {
        bool isInverted = invertControlsToggle.isOn;
        _playerControls.SetControlsDirection(isInverted);
        _audioSource.PlayOneShot(buttonPressed);
    }

    public void ContinueGame()
    {
        _audioSource.PlayOneShot(buttonPressed);
        _pauseGame.SetPauseState(false);
    }

    public void RestartLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(currentLevelIndex);
    }

    void LoadLevel(int levelIndex)
    {
        Time.timeScale = 1f;
        _audioSource.PlayOneShot(buttonPressed);
        SceneManager.LoadScene(levelIndex);
    }

    public void BackToMenu()
    {
        LoadLevel(0);
    }

    public void QuitGame()
    {
        _audioSource.PlayOneShot(buttonPressed);
        Application.Quit();
    }
}
