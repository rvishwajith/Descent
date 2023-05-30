using UnityEngine;
using System.Collections;

public class PortalCard : MonoBehaviour
{
    public Transform target;
    public Gradient colorGradient;
    public float
        maxDistance = 50f,
        minDistance = 5f;
    private Material material;
    private bool visible = false;

    private void Start()
    {
        material = transform.GetComponent<MeshRenderer>().material;
    }

    void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    void Update()
    {
        if (!visible) return;
        var distance = (target.position - transform.position).magnitude;
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        Color color = colorGradient.Evaluate(t);
        material.color = color;
    }
}