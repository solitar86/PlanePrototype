using Project.SFX;
using System;
using UnityEngine;

public class Deliverable : MonoBehaviour
{
    [SerializeField] Sound _pickUpSound;
    [SerializeField] private IslandColor _islandColor;
    [SerializeField] private Transform _parachute;
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _orangeMaterial;

    private Rigidbody _rigidBody;
    public IslandColor DeliverableColor => _islandColor;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerDeliveryHolder>(out PlayerDeliveryHolder holder))
        {
            holder.AddDeliverable(this);
            AudioPlayer.PlaySoundAtPoint(this, _pickUpSound, transform.position, true);
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (_rigidBody.IsSleeping() && _parachute.localScale != Vector3.zero)
        {
            _parachute.localScale = Vector3.Lerp(_parachute.localScale, Vector3.zero, Time.deltaTime);
        }
    }

    public void SetColor(IslandColor colorToAssign)
    {
        _islandColor = colorToAssign;

        switch (_islandColor)
        {
            case IslandColor.Blue:
                GetComponent<MeshRenderer>().material = _blueMaterial;
                break;
            case IslandColor.Yellow:
                GetComponent<MeshRenderer>().material = _yellowMaterial;
                break;
            case IslandColor.Green:
                GetComponent<MeshRenderer>().material = _greenMaterial;
                break;
            case IslandColor.Orange:
                GetComponent<MeshRenderer>().material = _orangeMaterial;
                break;
            case IslandColor.Red:
                GetComponent<MeshRenderer>().material = _redMaterial;
                break;
        }
    }

    public enum IslandColor
    {
        Blue,
        Yellow,
        Green,
        Orange,
        Red
    }
}
