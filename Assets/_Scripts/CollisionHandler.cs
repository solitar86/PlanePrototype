using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] private float _perfectLandingThreshold = 3f;
    [SerializeField] private float _smoothLandingThreshold = 6f;
    [SerializeField] private float _maxLandingThreshold = 9f;
    private void OnCollisionEnter(Collision collision)
    {
        var velocity = GetComponent<PlaneController_3>().Velocity;

        Debug.Log(velocity);

        //if (landingForce <  _perfectLandingThreshold )
        //{
        //    Debug.Log("Perfect Landing");
        //    return;
        //}
        //else if (landingForce < _smoothLandingThreshold)
        //{
        //    Debug.Log("Smooth Landing");
        //    return;
        //}
        //else if(landingForce < _maxLandingThreshold)
        //{
        //    Debug.Log("Rough Landing");
        //    return;
        //}

        Debug.Log("CRASHED");
    }
}
