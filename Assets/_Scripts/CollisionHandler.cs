using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] private float _perfectLandingThreshold = 3f;
    [SerializeField] private float _smoothLandingThreshold = 6f;
    [SerializeField] private float _maxLandingThreshold = 9f;
    [SerializeField] private Color _textColor = Color.white;
    private void OnCollisionEnter(Collision collision)
    {
        var velocity = GetComponent<PlaneController_3>().Velocity;
        float landingForce = Mathf.Abs(velocity.y);

        Debug.Log(landingForce);

        if (landingForce < _perfectLandingThreshold)
        {
            SpawnFloatingText("Perfect!");
            return;
        }
        else if (landingForce < _smoothLandingThreshold)
        {
            SpawnFloatingText("Smooth!");
            return;
        }
        else if (landingForce < _maxLandingThreshold)
        {
            SpawnFloatingText("Rough!");
            return;
        }

        SpawnFloatingText("CRASHED!");
    }

    private void SpawnFloatingText(string text)
    {
        FloatingText.Create(transform.position, text, _textColor);
    }
}
