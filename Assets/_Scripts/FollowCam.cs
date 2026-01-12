using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private Transform _followObject;
    private Vector3 _Posoffset;
    private Quaternion _rotOffset;

    private void Start()
    {
        _Posoffset = transform.position - _followObject.position;
    }

    private void LateUpdate()
    {
        transform.position = _Posoffset + _followObject.position;
        transform.rotation = _followObject.rotation;    
    }

}
