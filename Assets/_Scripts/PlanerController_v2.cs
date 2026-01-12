using UnityEngine;
using UnityEngine.InputSystem;

public class PlanerController_v2 : MonoBehaviour
{
   // [SerializeField] private float _turnSpeed = 10f;
    [SerializeField] private float _maxTurnSpeed = 100;
    private InputAction _turnAction;
    private InputAction _thrustAction;
    private Rigidbody _rigidbody;

    private float _currentYaw = 0f;
    private float _currentPitch = 0f;

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
        _currentYaw = Mathf.Lerp(0, _maxTurnSpeed, Time.deltaTime * _turnAction.ReadValue<Vector2>().x);
        _currentPitch = Mathf.Lerp(0, _maxTurnSpeed, Time.deltaTime * _turnAction.ReadValue<Vector2>().y);

        Debug.Log(_currentYaw + " : " + _currentPitch);
    }

    private void FixedUpdate()
    {
        _rigidbody.rotation *= Quaternion.Euler(_currentPitch, _currentYaw, 0);
    }

}
