using Utilities;
using UnityEngine;

public class RotateAroundParent : MonoBehaviour
{
    [Header("Rotation Options (Degrees)")]
    public float arcRadius = 2;
    public float arcSweep = 90f;
    public Vector3 arcAxis = Vector3.up;
    public float speed = 100f;

    private Transform pivot;

    private void Start()
    {
        pivot = new GameObject("RotationPivot").transform;
        pivot.parent = transform.parent;
        pivot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        transform.parent = pivot;
        transform.SetLocalPositionAndRotation(Vector3.forward * arcRadius, Quaternion.identity);
    }

    void Update()
    {
        float angle = arcSweep * Mathf.Sin(Convert.DegToRad(speed * Time.time));
        pivot.localRotation = Quaternion.Euler(arcAxis * angle);
    }
}
