using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    [SerializeField] MeshRenderer _getColorRenderer;
    [SerializeField] MeshRenderer _assignColorRenderer;
    void Start()
    {
        _assignColorRenderer.material = _getColorRenderer.material;
    }


}
