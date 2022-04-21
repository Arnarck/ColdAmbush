using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject winMenu;
    [Header("Audio Clips")]
    [SerializeField] AudioClip menuSFX;

    PlayerCollisions _player;
    AudioSource _audioSource;
    MusicBehaviour _musicBehaviour;
    AftermatchScoreboard _aftermatchScoreboard;

    bool _isGamePaused;
    bool _isGameFinished;

    // Start is called before the first frame update
    void Start()
    {
        _musicBehaviour = FindObjectOfType<MusicBehaviour>();
        _aftermatchScoreboard = FindObjectOfType<AftermatchScoreboard>();
        _audioSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCollisions>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPauseInput();
    }

    void ProcessPauseInput()
    {
        bool isPauseInputPressed = (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7));
        bool canChangePauseState = (isPauseInputPressed && _player.IsAlive() && !_isGameFinished);

        if (canChangePauseState)
        {
            SetPauseState(!_isGamePaused); // if the game is paused, unpause it. If the game is unpaused, pause it
        }
    }

    public void SetPauseState(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        _isGamePaused = isPaused;

        _musicBehaviour.SetMusicVolume(isPaused);
        _audioSource.PlayOneShot(menuSFX);
        pauseMenu.SetActive(isPaused);
    }

    public void ActivateGameOver()
    {
        if (_player.IsAlive()) { return; } // "game over" only occurs when the player is dead

        FinishGame(gameOverMenu);
    }

    public void ActivateWin()
    {
        if (!_player.IsAlive()) { return; } // "win" only occurs when the player is alive

        FinishGame(winMenu);
    }

    void FinishGame(GameObject menu)
    {
        Time.timeScale = 0f;
        _isGamePaused = true;
        _isGameFinished = true;

        _musicBehaviour.SetMusicVolume(true);
        _audioSource.PlayOneShot(menuSFX);
        _aftermatchScoreboard.SetHighScore();
        _aftermatchScoreboard.UpdateFinalScoreboard();
        menu.SetActive(true);

    }

    public bool IsPaused()
    {
        return _isGamePaused;
    }
}
