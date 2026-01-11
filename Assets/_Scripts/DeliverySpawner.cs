using System;
using UnityEngine;

public class DeliverySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private BoxCollider[] _spawnAreas;
    [SerializeField] private Transform _deliveryPrefab;

    System.Random RNG = new System.Random(999);

    private void Start()
    {
        InvokeRepeating(nameof(SpawnDelivery), _spawnInterval, _spawnInterval);
    }

    private void SpawnDelivery()
    {
        int index = RNG.Next(0, _spawnAreas.Length - 1);

        SpawnDeliveryWithinSpawnArea(_spawnAreas[index]);
    }

    private void SpawnDeliveryWithinSpawnArea(BoxCollider boxCollider)
    {
        var position = RandomPointInBoxCollider(boxCollider);
        position += Vector3.up * 15f;
        Instantiate(_deliveryPrefab, position, Quaternion.identity);
    }

    public Vector3 RandomPointInBoxCollider(BoxCollider box)
    {
        Vector3 center = box.center;
        Vector3 extents = box.size * 0.5f;

        // Random point in local space
        Vector3 localPoint = new Vector3(
            NextFloat(RNG, -extents.x, extents.x),
            NextFloat(RNG, -extents.y, extents.y),
            NextFloat(RNG, -extents.z, extents.z)
        ) + center;

        // Convert to world space
        return box.transform.TransformPoint(localPoint);
    }

    public float NextFloat(System.Random rng, float min, float max)
    {
        return min + (float)rng.NextDouble() * (max - min);
    }
}
