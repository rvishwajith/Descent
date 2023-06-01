using UnityEngine;
using System.Collections;

public class SnakePath : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target;
    public float moveSpeed = 2.5f;

    [Header("Node")]
    public Transform prefab;
    public int numSegments = 4;
    public float segmentDistance = 2f;

    private Vector3 prevHeadPos;
    private Transform[] nodes;
    private Transform head;

    void Awake()
    {
        /*
        nodes = new Transform[numSegments];
        for (var i = 0; i < numSegments; i++)
        {
            var node = GameObject.Instantiate(prefab);
            node.position = transform.position - transform.forward * (i * segmentDistance);
            node.name = "Node " + i;
            node.parent = transform;
            nodes[i] = node;
        }
        */
        nodes = new Transform[transform.childCount];
        for (var i = 0; i < nodes.Length; i++)
        {
            nodes[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        bool move = true;
        if (move)
        {
            prevHeadPos = head.position;
            MoveHead();
            MoveFollowers();
            ConstrainFollowers();
        }
    }

    void MoveHead()
    {
        //var targetPos = target.position;
        var posDelta = target.position - head.position;
        var distance = posDelta.magnitude;
        if (distance > Time.deltaTime * moveSpeed)
        {
            head.position += posDelta.normalized * Time.deltaTime * moveSpeed;
        }
        else
        {
            head.position = target.position;
        }
    }

    void MoveFollowers()
    {
        // If the head moved, move the followers towards their target.
        bool moved = head.position != prevHeadPos;
        if (!moved) return;

        // How much did the head move? Move followers the same amount.
        var distanceMoved = (head.position - prevHeadPos).magnitude;

        for (var i = 1; i < nodes.Length; i++)
        {
            var target = nodes[i - 1];
            var node = nodes[i];
            MoveFollowerNode(node, target, distanceMoved);
        }
    }

    void MoveFollowerNode(Transform node, Transform target, float distance)
    {
        var posDelta = target.position - node.position;
        node.position += posDelta.normalized * distance;
    }

    void ConstrainFollowers()
    {
        for (var i = 1; i < nodes.Length; i++)
        {
            var target = nodes[i - 1];
            var node = nodes[i];
            ConstrainFollowerNode(node, target, segmentDistance);
        }
    }

    void ConstrainFollowerNode(Transform node, Transform target, float distance)
    {
        var deltaPos = target.position - node.position;
        node.position = target.position;
        node.position -= deltaPos.normalized * segmentDistance;
    }
}

