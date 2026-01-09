using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController_3 : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _maxSpeed = 100f;
    [SerializeField] private float _maxPitch = 30f;
    [SerializeField] private float _minThrustToDisableGravity = 3f;
    [SerializeField] private float _minThrustToTurn = 0.5f;
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [Space(15)]
    [SerializeField] private float _resetSpeed = 3f;
    [Space(15)]
    [SerializeField] private Transform _planeVisual;
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

        if (turnVector != Vector2.zero)
        {
            if(_canTurn)
            {
                _currentYaw += turnVector.x * Time.deltaTime * _turnSpeed;
            }

            if (_hasLift)
            {
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
            planeVisualRotation.z = 0f;
            planeVisualRotation.x = 0f;
        }

        _planeVisual.localRotation = Quaternion.Lerp(_planeVisual.localRotation,
                                                        Quaternion.Euler(planeVisualRotation),
                                                        Time.deltaTime * _planeVisualRotationSpeed);


        _currentThrust += _thrustAction.ReadValue<float>() * _acceleration * Time.deltaTime;
        _currentThrust = Mathf.Clamp(_currentThrust, 0, _maxSpeed);

        _hasLift = _currentThrust > _minThrustToDisableGravity;
        _canTurn = Canturn();
        _canRoll = CanRoll();

    }

    private void FixedUpdate()
    {
        if (_hasLift)
        {
            // Rotation to face movement direction if we "have lift"
            _rigidbody.rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
        }
        else if (_canTurn && _canRoll)
        {
            _rigidbody.rotation = Quaternion.Euler(0, _currentYaw, 0);
        }

        // Calculate current velocity vector.

        Vector3 velocity = _rigidbody.transform.forward * _currentThrust;

        // Apply gravity
        //float gravity = Physics.gravity.y;
        //gravity = Mathf.Clamp(gravity, Physics.gravity.y + _currentThrust, 0);
        //velocity.y = gravity;
        //Debug.Log(gravity);

        // Move plane
        _rigidbody.position += velocity * Time.fixedDeltaTime;

        _rigidbody.useGravity = !_hasLift;

        
    }

    private bool Canturn()
    {
        return _currentThrust > _minThrustToTurn;
    }

    private bool CanRoll()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance, Color.red);

        Ray groundRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(groundRay, out RaycastHit hitInfo, _groundCheckDistance) == false)
        {
            return true;
        }

        // We are too close to ground
        return false;
    }



    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 100, 50), "<color=#000000>Thrust: " + _currentThrust.ToString("F2") + "</color>");
    }
}
