using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DeliverySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private BoxCollider[] _spawnAreas;
    [SerializeField] private Transform _deliveryPrefab;
    [SerializeField] private int _seed;
    System.Random RNG;


    IEnumerator Start()
    {

        RNG = new System.Random(_seed);
        yield return new WaitWhile(() => GameManager._HasGameStarted == false);
        SpawnDelivery();
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
        var go = Instantiate(_deliveryPrefab, position, Quaternion.identity);
        var delivery = go.GetComponent<Deliverable>();
        var colorToAvoid = boxCollider.GetComponent<IslandSurface>().IslandColor;

        var colorToAssign = GetRandomIslandColorWithExlusion(colorToAvoid);
        delivery.SetColor(colorToAssign);

    }

    private Deliverable.IslandColor GetRandomIslandColorWithExlusion(Deliverable.IslandColor colorToAvoid)
    {
        var colorToReturn = Deliverable.IslandColor.Blue;
        var values = Deliverable.IslandColor.GetValues(typeof(Deliverable.IslandColor));

        do
        {
            colorToReturn = (Deliverable.IslandColor)values.GetValue(RNG.Next(values.Length));
        } while (colorToReturn == colorToAvoid);
        return colorToReturn;
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
