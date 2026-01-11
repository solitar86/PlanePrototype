using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WingTipAirTrailLine : MonoBehaviour
{
    [Header("Trail Shape")]
    public int maxPoints = 30;
    public float pointSpacing = 0.1f;

    private LineRenderer line;
    private Vector3[] points;
    private Vector3 lastPosition;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        points = new Vector3[maxPoints];
        lastPosition = transform.position;

        // Initialize all points at start position
        for (int i = 0; i < maxPoints; i++)
            points[i] = transform.position;

        line.positionCount = maxPoints;
        line.SetPositions(points);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        if (distance < pointSpacing)
            return;

        lastPosition = transform.position;
        ShiftAndAddPoint(transform.position);
    }

    void ShiftAndAddPoint(Vector3 newPoint)
    {
        // Shift all points back (FIFO)
        for (int i = 0; i < maxPoints - 1; i++)
            points[i] = points[i + 1];

        // Add newest point at the end
        points[maxPoints - 1] = newPoint;

        line.SetPositions(points);
    }
}
