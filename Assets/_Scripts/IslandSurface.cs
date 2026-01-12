using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class IslandSurface: MonoBehaviour
{
    [SerializeField] public Deliverable.IslandColor IslandColor;
}