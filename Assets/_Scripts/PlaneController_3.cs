using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController_3 : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _accelerationMultiplier = 10f;
    [SerializeField] private float _maxSpeed = 100f;
    [SerializeField] private float _maxPitch = 30f;
    [SerializeField] private float _minThrustToDisableGravity = 3f;
    [SerializeField] private float _minThrustToTurn = 0.5f;
    [SerializeField] private float _canRollGroundCheckDistance = 1.2f;
    [SerializeField] private float _groundCheckDistance = 0.6f;
    [SerializeField] private float _planeGravity = 10f;
    [SerializeField] private float _planeGroundFiction = 10f;
    [Space(15)]
    [SerializeField] private float _resetSpeed = 3f;
    [Space(15)]
    [SerializeField] private Transform _planeVisual;
    [SerializeField] private Transform _propeller;
    [SerializeField] private float _propellerSpeed = 10f;
    [SerializeField] private float _planeVisualMaxPitch = 30f;
    [SerializeField] private float _planeVisualMaxRoll = 30f;
    [SerializeField] private float _planeVisualRotationSpeed = 10f;
    private InputAction _turnAction;
    private InputAction _thrustAction;
    private Rigidbody _rigidbody;

    private float _currentYaw = 0f;
    private float _currentPitch = 0f;
    private float _currentThrust = 0f;

    private bool _hasLift = false;
    private bool _canTurn = false;
    private bool _canRoll = false;
    private bool _isOnGround = false;

    public Vector3 Velocity;

    private Vector3 planeVisualRotation = Vector3.zero;

    public float ThrustNormalized => _currentThrust / _maxSpeed;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }
    void Start()
    {
        _turnAction = InputSystem.actions.FindAction("Turn");
        _thrustAction = InputSystem.actions.FindAction("Thrust");
        _thrustAction.Enable();
        _turnAction.Enable();
    }

    private void Update()
    {
        Vector2 turnVector = _turnAction.ReadValue<Vector2>();

        // If player is trying to turn, handle that.
        if (turnVector != Vector2.zero)
        {
            if(_canTurn)
            {
                // We are going fast enough to allow player to turn plane (even on ground)
                _currentYaw += turnVector.x * Time.deltaTime * _turnSpeed;
            }

            if (_hasLift)
            {
                // We are going fast enough to tilt up off of ground?
                _currentPitch += turnVector.y * Time.deltaTime * _turnSpeed;
                _currentPitch = Mathf.Clamp(_currentPitch, -_maxPitch, _maxPitch);

                if(_canRoll)
                {
                    planeVisualRotation.z = -turnVector.x * _planeVisualMaxRoll;
                }
                planeVisualRotation.x = turnVector.y * _planeVisualMaxPitch;
            }
        }
        else
        {
            // Reset movement direction pitch slowly over time.
            _currentPitch = Mathf.Lerp(_currentPitch, 0, Time.deltaTime * _resetSpeed * 0.25f);

            planeVisualRotation.z = 0f;
            planeVisualRotation.x = 0f;
        }

        if(_canRoll == false)
        {
            // We are too close to ground so level out visual rotation so wings don't touch ground.
            planeVisualRotation.z = 0f;
            planeVisualRotation.x = 0f;
        }

        // Rotate plane visual to match player input.
        _planeVisual.localRotation = Quaternion.Lerp(_planeVisual.localRotation,
                                                        Quaternion.Euler(planeVisualRotation),
                                                        Time.deltaTime * _planeVisualRotationSpeed);

        // Apply player input to current thrust and clamp value
        _currentThrust += _thrustAction.ReadValue<float>() * _accelerationMultiplier * Time.deltaTime;

        if (_isOnGround && _thrustAction.ReadValue<float>() < 0.1f)
        {
            // Player is not accelerating but we are touching ground, slow down.
            _currentThrust -= _planeGroundFiction * Time.deltaTime;
        }

        _currentThrust = Mathf.Clamp(_currentThrust, 0, _maxSpeed);

        _hasLift = _currentThrust > _minThrustToDisableGravity;
        _canTurn = CanTurn();
        _canRoll = CanRoll();
        _isOnGround = IsOnGround();


        _propeller.localRotation *= Quaternion.Euler(0, 0, _propellerSpeed * _currentThrust * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        if (_hasLift)
        {
            // Rotation to face movement direction if we "have lift"
            Quaternion targetRotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation,
                                                    targetRotation,
                                                    Time.fixedDeltaTime * _resetSpeed);
        }
        else if (_canTurn)
        {
            //We can't lift off but we can turn on the ground. 
            Quaternion targetRotation = Quaternion.Euler(0, _currentYaw, 0);
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation,
                                                    targetRotation,
                                                    Time.fixedDeltaTime * _resetSpeed);
        }

        // Calculate current velocity vector based on plane heading.
        Vector3 velocity = _rigidbody.transform.forward * _currentThrust;

        // Apply gravity
        float downForceLerpValue = Mathf.InverseLerp(0, _maxSpeed, _currentThrust);
        float gravityLerped = Mathf.Lerp(_planeGravity, 0, downForceLerpValue);

        if (_currentThrust < 0.1f && _isOnGround == false) gravityLerped = 5f;
        velocity += new Vector3(0, -gravityLerped, 0);

        // Move plane
        _rigidbody.position += velocity * Time.fixedDeltaTime;

        //_rigidbody.useGravity = !_hasLift;
        _rigidbody.useGravity = false;


        Velocity = velocity;

        //if(_isOnGround && _hasLift == false)
        //{
        //    // We are on ground, lerp plane upright.
        //    Quaternion targetRotation = Quaternion.Euler(0, _currentYaw, 0);
        //    _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation,
        //                                            targetRotation,
        //                                            Time.fixedDeltaTime * _resetSpeed);
        //}
    }

    private bool CanTurn()
    {
        return _currentThrust > _minThrustToTurn;
    }

    private bool CanRoll()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * _canRollGroundCheckDistance, Color.red);

        Ray groundRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(groundRay, out RaycastHit hitInfo, _canRollGroundCheckDistance) == false)
        {
            // We are above ground
            return true;
        }

        // We are too close to ground
        return false;
    }

    private bool IsOnGround()
    {
        // We are checking not objectively down but down based on plane
        Debug.DrawLine(transform.position, transform.position + transform.up *-1f * _canRollGroundCheckDistance, Color.yellow);

        Ray groundRay = new Ray(transform.position, transform.up * -1);

        if (Physics.Raycast(groundRay, out RaycastHit hitInfo, _groundCheckDistance))
        {
            // Bottom of plane is touching ground
            return true;
        }

        return false;
    }


    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 100, 50), "<color=#000000>Thrust: " + _currentThrust.ToString("F2") + "</color>");
        GUI.Label(new Rect(25, 60, 100, 50), "<color=#000000>Velocity: " + _rigidbody.linearVelocity.ToString("F2") + "</color>");
    }
}
