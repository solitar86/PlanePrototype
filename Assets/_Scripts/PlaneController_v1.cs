using UnityEngine;
using UnityEngine.InputSystem;


public class PlaneController_v1 : MonoBehaviour
{
    [SerializeField] private float _planeMass = 350f;
    [SerializeField] private float _maxThrust = 100;
    [SerializeField] private float _turnForce = 100f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _angularDamping = 0.5f;
    [SerializeField] private float _linearDamping = 0.5f;
    [SerializeField] private float _lift = 100f;
    [Space(15)]
    [SerializeField] private Transform _propeller;

    private float _currenThrust = 0f;

    private Rigidbody _rigidbody;
    private InputAction _turnAction;
    private InputAction _thrustAction;
    private Vector2 _inputVector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _turnAction = InputSystem.actions.FindAction("Turn");
        _thrustAction = InputSystem.actions.FindAction("Thrust");
        _thrustAction.Enable();
        _turnAction.Enable();
    }


    private void Update()
    {
        _inputVector = _turnAction.ReadValue<Vector2>();
        _currenThrust += _thrustAction.ReadValue<float>() * _acceleration * Time.deltaTime;
        _currenThrust = Mathf.Clamp(_currenThrust, 0, _maxThrust);

        _propeller.Rotate(_currenThrust * Time.deltaTime, 0, 0);
    

    }

    private void FixedUpdate()
    {
        _rigidbody.angularDamping = _angularDamping;
        _rigidbody.linearDamping = _linearDamping;
        _rigidbody.mass = _planeMass;

        var thrustVector = transform.forward * _maxThrust * _currenThrust;
        thrustVector += Vector3.up * _lift;
        _rigidbody.AddTorque(transform.forward * -_inputVector.x * _turnForce, ForceMode.Force);
        _rigidbody.AddTorque(transform.right * _inputVector.y * _turnForce, ForceMode.Force);
        _rigidbody.AddForce(thrustVector);
    }

    // DEBUGGING
    void OnGUI()
    { 
        GUI.Label(new Rect(25, 25, 100, 50), "Thrust: " + _currenThrust.ToString("F2"));
    }
}
