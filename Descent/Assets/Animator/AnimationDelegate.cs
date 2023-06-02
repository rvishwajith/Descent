using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelegate : MonoBehaviour
{
    private List<AnimationNode> animations = new();

    private void Awake()
    {
        Delegates.Animation = this;
    }

    public void Animate(Transform transform, string property = "Position",
        System.Object start = null, System.Object end = null,
        float duration = 0, float delay = 0, int easing = 0)
    {
        AnimationNode node = new();
        if (start == null) start = transform.position;
        if (end == null) end = transform.position;

        node.transform = transform;
        node.property = property;
        node.start = start;
        node.end = end;
        node.startTime = Time.time + delay;
        node.endTime = Time.time + delay + duration;
        node.easing = 0;
        Debug.Log(node.startTime + ", " + node.endTime);
        animations.Add(node);
    }

    void Update()
    {
        int i = 0;
        while (i < animations.Count)
        {
            var animation = animations[i];
            if (Time.time < animation.startTime) continue;

            var t = Mathf.InverseLerp(animation.startTime, animation.endTime, Time.time);
            UpdateProperty(animation, t);

            if (animation.completed)
            {
                animations.RemoveAt(i);
                Debug.Log("Complated animation!");
            }
            else i++;
        }
    }

    void UpdateProperty(AnimationNode animation, float t)
    {
        var transform = animation.transform;
        if (animation.property == "Position")
        {
            transform.position = Vector3.Lerp(
                (Vector3)animation.start,
                (Vector3)animation.end, Easing(t));
        }
        else if (animation.property == "LocalPosition")
        {
            transform.localPosition = Vector3.Lerp(
                (Vector3)animation.start,
                (Vector3)animation.end, Easing(t));
        }
        else if (animation.property == "EulerAngles")
        {
            transform.eulerAngles = Vector3.Lerp(
                (Vector3)animation.start,
                (Vector3)animation.end, Easing(t));
        }
        else if (animation.property == "LocalEulerAngles")
        {
            transform.localEulerAngles = Vector3.Lerp(
                (Vector3)animation.start,
                (Vector3)animation.end, Easing(t));
        }
        if (t >= 1) animation.completed = true;
    }

    private static float Easing(float t, int mode = 0)
    {
        if (mode == 0) return t; // Linear
        else if (mode == 1) return t; // Quadratic
        else if (mode == 2) return t; // Cubic
        else if (mode == 3) return t; // Smoothstep
        return t;
    }
}

struct AnimationNode
{
    public Transform transform;
    public string property;
    public System.Object start, end;
    public float startTime, endTime;
    public int easing;
    public bool completed;
    public bool looping;
}