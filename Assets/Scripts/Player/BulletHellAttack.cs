using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BulletHellAttack : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject projectiles;
    [SerializeField] GameObject muzzleFlash;

    [Header("Sliders")]
    [SerializeField] Slider progressBar;

    [Header("Audio Clips")]
    [SerializeField] AudioClip attackActivated;
    [SerializeField] AudioClip attackUnlocked;
    [SerializeField] AudioClip attackLocked;

    [Header("Settings")]
    [SerializeField] int maxBarValue = 100;
    [SerializeField] float coolDown = 0.15f;
    
    Coroutine _consumeBar;
    PlayerCollisions _player;
    AudioSource _audioSource;
    
    int _currentBarValue;

    bool _isProjectilesActivated;
    bool _canActivateProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GameObject.FindWithTag("GameSystem").GetComponent<AudioSource>();
        _player = GetComponent<PlayerCollisions>();
        progressBar.maxValue = maxBarValue;
        progressBar.value = _currentBarValue;
    }

    // Charges the progress bar by certain amount
    public void Charge(int amount)
    {
        if (_isProjectilesActivated || !_player.IsAlive() || _canActivateProjectiles) { return; }

        _currentBarValue += amount;
        if (_currentBarValue >= maxBarValue)
        {
            _currentBarValue = maxBarValue;
            _canActivateProjectiles = true;
            _audioSource.PlayOneShot(attackUnlocked);
        }

        progressBar.value = _currentBarValue;
    }

    // if the lasers are activated, they will be deactivated, and vice-versa
    public void ToggleActiveState()
    {
        if (_isProjectilesActivated)
        {
            StopProjectiles();
        }
        else
        {
            ActivateProjectiles();
        }

    }

    // Stop the lasers without reset the progress bar
    void StopProjectiles()
    {
        SetProjectilesActive(false, attackLocked);
        StopCoroutine(_consumeBar);
    }

    // Activate/deactivate the lasers
    void SetProjectilesActive(bool isActive, AudioClip sfx)
    {
        _isProjectilesActivated = isActive;
        projectiles.SetActive(isActive);
        muzzleFlash.SetActive(isActive);
        _audioSource.PlayOneShot(sfx);
    }

    // Start shooting and consuming the progress bar
    void ActivateProjectiles()
    {
        if (!_canActivateProjectiles) { return; }

        _canActivateProjectiles = false;
        SetProjectilesActive(true, attackActivated);
        _consumeBar = StartCoroutine(ConsumeBar());
    }

    // Consumes the progress bar over time
    IEnumerator ConsumeBar()
    {
        while (_isProjectilesActivated)
        {
            _currentBarValue -= 1;
            if (_currentBarValue < 1)
            {
                DeactivateProjectiles();
            }

            progressBar.value = _currentBarValue;
            yield return new WaitForSeconds(coolDown);
        }
    }

    // Stop the lasers AFTER the progress bar be completely consumed
    void DeactivateProjectiles()
    {
        _currentBarValue = 0;
        SetProjectilesActive(false, attackLocked);
    }
}
