using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float throwFactor = 10f;
    [SerializeField] float xRange = 10f;
    [SerializeField] float yRange = 9f;

    [Header("Position Factor")]
    [SerializeField] float positionPitchFactor = 2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Movement Factor")]
    [SerializeField] float movementPitchFactor = 3f;
    [SerializeField] float movementRollFactor = 3f;

    [Header("Lasers")]
    [SerializeField] LaserSpawner[] laserSpawners;
    [SerializeField] GameObject[] muzzleFlashes;

    BulletHellAttack _bulletHellAttack;
    PauseGame _pauseGame;

    float xInput, yInput;

    bool _isControlsInverted;

    // Start is called before the first frame update
    void Start()
    {
        _bulletHellAttack = GetComponent<BulletHellAttack>();
        _pauseGame = FindObjectOfType<PauseGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pauseGame.IsPaused())
        {
            RecordMovementInputThisFrame();
            ProcessTranslation();
            ProcessRotation();
            ProcessFireInput();
            ProcessBulletHellAttackInput();
        }
    }

    void RecordMovementInputThisFrame()
    {
        int yControlsDirection = _isControlsInverted ? -1 : 1;

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical") * yControlsDirection;
    }

    void ProcessTranslation()
    {
        float xThrowThisFrame = xInput * Time.deltaTime * throwFactor;
        float yThrowThisFrame = yInput * Time.deltaTime * throwFactor;

        float xRaw = transform.localPosition.x + xThrowThisFrame;
        float yRaw = transform.localPosition.y + yThrowThisFrame;

        float xClamped = Mathf.Clamp(xRaw, -xRange, xRange);
        float yClamped = Mathf.Clamp(yRaw, -yRange, yRange);

        transform.localPosition = new Vector3(xClamped, yClamped, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        // pitch -> rotates x
        //yaw -> rotates y
        //roll -> rotates z
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float yawDueToPosition = transform.localPosition.x * positionYawFactor;

        float pitchDueToMovement = yInput * movementPitchFactor;
        float rollDueToMovement = xInput * movementRollFactor;

        float pitchThisFrame = pitchDueToPosition + pitchDueToMovement;

        transform.localRotation = Quaternion.Euler(pitchThisFrame, yawDueToPosition, rollDueToMovement);
    }

    void ProcessFireInput()
    {
        bool isFireButtonPressed = Input.GetButton("Fire1") || Input.GetKey(KeyCode.RightShift);
        if (isFireButtonPressed)
        {
            ShootLasers();
        }

        SetMuzzleFlashesActive(isFireButtonPressed);
    }

    void ShootLasers()
    {
        foreach (LaserSpawner laserSpawner in laserSpawners)
        {
            if (laserSpawner.isActiveAndEnabled)
            {
                laserSpawner.Shoot();
            }
        }
    }

    void SetMuzzleFlashesActive(bool isActive)
    {
        foreach(GameObject muzzleFlash in muzzleFlashes)
        {
            muzzleFlash.SetActive(isActive);
        }
    }

    void ProcessBulletHellAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            _bulletHellAttack.ToggleActiveState();
        }
    }

    public void SetControlsDirection(bool isInverted)
    {
        _isControlsInverted = isInverted;
    }
}
