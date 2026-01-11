using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_CollisionHandler : MonoBehaviour
{

    [SerializeField] private float _perfectLandingThreshold = 3f;
    [SerializeField] private float _smoothLandingThreshold = 6f;
    [SerializeField] private float _maxLandingThreshold = 9f;
    [SerializeField] private Color _textColor = Color.white;

    private bool _isFlying;
    private bool _isTakingOff;
    private bool _isCrashed;

    [SerializeField] private float crashTestDistance = 0.25f;
    [SerializeField] private Transform[] _crashRaycastPoints;

    private float timer = 0f, timerMax = 1.5f;
    private PlaneController_3 _playerController;

    public enum LandingType
    {
        Perfect,
        Smooth,
        Rough,
        Crash
    }

    public LandingType lastLandingType;

    private void Start()
    {
        _playerController = GetComponent<PlaneController_3>();
    }
    private void Update()
    {
        if (_isTakingOff == true)
        {
            timer += Time.deltaTime;
            if (timer > timerMax)
            {
                _isCrashed = false;
                _isFlying = true;
                _isTakingOff = false;
                timer -= timerMax;
            }
        }
    }

    public bool IsLanded() => _isFlying == false &&
                                _isTakingOff == false &&
                                _isCrashed == false &&
                                _playerController.ThrustNormalized == 0;
    public bool IsCrashed() => _isCrashed == true;

    private void OnCollisionExit(Collision collision)
    {
        _isTakingOff = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Deliverable>(out Deliverable deliverable))
        {
            Debug.Log("Deliverable");
        }
        else
        {
            HandleCollisionsWithEnvironment(collision);
        }
    }

    private void HandleCollisionsWithEnvironment(Collision collision)
    {
        if (_isFlying == false) return;
        _isFlying = false;

        if (collision.gameObject.tag == "Water")
        {
            CrashedPlane(collision);
            return;
        }


        var velocity = GetComponent<PlaneController_3>().Velocity;
        float landingForce = Mathf.Abs(velocity.y);

        if (landingForce < _perfectLandingThreshold)
        {
            SpawnFloatingText("Perfect!");
            lastLandingType = LandingType.Perfect;
            return;
        }
        else if (landingForce < _smoothLandingThreshold)
        {
            SpawnFloatingText("Smooth!");
            lastLandingType = LandingType.Smooth;
            return;
        }
        else if (landingForce < _maxLandingThreshold)
        {
            SpawnFloatingText("Rough!");
            lastLandingType = LandingType.Rough;
            return;
        }

        CrashedPlane(collision);
    }

    private void CrashedPlane(Collision collision)
    {
        _isCrashed = true;
        lastLandingType = LandingType.Crash;
        SpawnFloatingText("CRASHED into: " + collision == null ? collision.gameObject.name : "null");
    }

    private void FixedUpdate()
    {
        foreach (var point in _crashRaycastPoints)
        {
            Debug.DrawLine(point.position, point.position + point.forward * crashTestDistance);
            if (Physics.Raycast(point.position, point.forward, out RaycastHit hitInfo, crashTestDistance))
            {
                if (hitInfo.collider.gameObject.TryGetComponent<Deliverable>(out Deliverable _))
                {
                    // Raycasting into deliverable
                }
                else
                {
                    CrashedPlane(null);
                }
            }
        }
    }


    private void SpawnFloatingText(string text)
    {
        FloatingText.Create(transform.position, text, _textColor);
    }

    private void OnDrawGizmos()
    {
        foreach (var point in _crashRaycastPoints)
        {
            Debug.DrawLine(point.position, point.position + point.forward * crashTestDistance);
        }
    }
}
