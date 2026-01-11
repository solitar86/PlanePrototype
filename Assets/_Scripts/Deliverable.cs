using Project.SFX;
using UnityEngine;

public class Deliverable : MonoBehaviour
{
    [SerializeField] Sound _pickUpSound;
    [SerializeField] private Color _type;

    [SerializeField] private Transform _parachute;

    private Rigidbody _rigidBody;
    public Color DeliverableColor => _type;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerDeliveryHolder>(out PlayerDeliveryHolder holder))
        {
            holder.AddDeliverable(this);
            AudioPlayer.PlaySoundAtPoint(this, _pickUpSound, transform.position);
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

    public enum Color
    {
        Blue,
        Yellow,
        Green,
        Orange,
        Red
    }
}
