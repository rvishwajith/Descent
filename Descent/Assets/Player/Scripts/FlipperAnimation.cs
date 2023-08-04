using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipperAnimation : MonoBehaviour
{
    private Matrix4x4[] oldMatrices;

    // [SerializeField] private Transform root;
    [SerializeField] private Transform baseTransform;
    [SerializeField] private List<FlipperNode> nodes = new();

    private void Awake()
    {
        foreach (var node in nodes)
        {
            node.transform.SetParent(null);
        }
    }

    private void Update()
    {
        foreach (var node in nodes)
        {
            node.Update(Time.deltaTime);
        }
    }

    private void OnValidate()
    {
        if (baseTransform == null) baseTransform = transform;

        if (Application.isPlaying) return;

        nodes = new();
        var currParent = baseTransform.parent;
        while (currParent.childCount > 0)
        {
            var node = new FlipperNode();
            node.Init(currParent.GetChild(0));
            nodes.Add(node);
            currParent = currParent.GetChild(0);
        }
    }
}

[Serializable]
public class FlipperNode
{
    public Transform transform;
    public Transform parent;

    [HideInInspector] public Vector3 resetDir { get { return parent.forward; } }
    [HideInInspector] public float resetForce;
    [HideInInspector] public float segmentLength;
    [HideInInspector] public bool isAnchor;

    public void Init(Transform t, bool anchor = false)
    {
        transform = t;
        parent = transform.parent;
        isAnchor = anchor;
        resetForce = 3.5f;
        segmentLength = Vector3.Distance(transform.position, parent.position);
    }

    public void Update(float dT)
    {
        transform.position += resetDir * Time.deltaTime * resetForce;

        // var dist = Vector3.Distance(transform.position, parent.position);
        var offsetDir = (transform.position - parent.position).normalized;
        transform.position = parent.position + segmentLength * offsetDir;

        transform.LookAt(transform.position + resetDir);
    }
}