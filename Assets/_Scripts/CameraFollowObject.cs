using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    private void Update()
    {
       transform.position = playerTransform.position;
       transform.rotation = playerTransform.rotation;
    }
}
